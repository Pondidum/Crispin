using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public abstract class StorageSessionTests : IAsyncLifetime
	{
		protected IStorageSession Session { get; private set; }
		protected Dictionary<Type, Func<IEnumerable<IEvent>, object>> Builders { get; }
		protected List<Projector> Projections { get; }
		protected EditorID Editor { get; }

		protected StorageSessionTests()
		{
			Builders = new Dictionary<Type, Func<IEnumerable<IEvent>, object>>();
			Projections = new List<Projector>();
			Editor = EditorID.Parse("wat");

			Builders[typeof(Toggle)] = events => AggregateBuilder.Build(new Toggle(), events);
		}

		public async Task InitializeAsync() => Session = await CreateSession();

		protected abstract Task<IStorageSession> CreateSession();

		protected abstract Task<bool> AggregateExists(ToggleID toggleID);
		protected abstract Task WriteEvents(ToggleID toggleID, params IEvent[] events);
		protected abstract Task<IEnumerable<Type>> ReadEvents(ToggleID toggleID);
		protected abstract Task<IEnumerable<TProjection>> ReadProjection<TProjection>();

		protected void AddDefaultProjection()
		{
			Projections.Add(new Projector(typeof(ToggleView), () => new ToggleView()));
		}

		[Fact]
		public void When_there_is_no_builder_for_an_aggregate()
		{
			Builders.Clear();

			Should.Throw<BuilderNotFoundException>(() => Session.LoadAggregate<Toggle>(ToggleID.CreateNew()));
		}

		[Fact]
		public void When_there_is_no_aggregate_stored()
		{
			Should.Throw<AggregateNotFoundException>(() => Session.LoadAggregate<Toggle>(ToggleID.CreateNew()));
		}

		[Fact]
		public async Task When_there_are_no_events_for_an_aggregate_stored()
		{
			var toggleID = ToggleID.CreateNew();
			await WriteEvents(toggleID);

			Should.Throw<AggregateNotFoundException>(() => Session.LoadAggregate<Toggle>(toggleID));
		}

		[Fact]
		public async Task When_an_aggregate_is_loaded()
		{
			var toggleID = ToggleID.CreateNew();

			await WriteEvents(
				toggleID,
				new ToggleCreated(Editor, toggleID, "First", "hi", ConditionModes.All).AsAct(toggleID),
				new TagAdded(Editor, "one").AsAct(toggleID)
			);

			var toggle = await Session.LoadAggregate<Toggle>(toggleID);

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(toggleID),
				() => toggle.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public async Task When_saving_an_aggregate_and_commit_is_not_called()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");

			await Session.Save(toggle);

			var exists = await AggregateExists(toggle.ID);

			exists.ShouldBeFalse();
		}

		[Fact]
		public async Task When_saving_an_aggregate_and_commit_is_called()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");

			await Session.Save(toggle);
			await Session.Commit();

			var events = await ReadEvents(toggle.ID);

			events.ShouldBe(new[]
			{
				typeof(Event<ToggleCreated>),
				typeof(Event<TagAdded>),
			});
		}

		[Fact]
		public async Task When_loading_an_aggregate_saved_in_the_current_session()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");

			await Session.Save(toggle);

			var loaded = await Session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public async Task When_loading_an_aggregate_existing_in_store_and_saved_in_the_current_session()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");

			await Session.Save(toggle);
			await Session.Commit();

			toggle.AddTag(Editor, "two");
			await Session.Save(toggle);

			var loaded = await Session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.Tags.ShouldContain("one", "two")
			);
		}

		[Fact]
		public async Task When_there_are_pending_events_and_dispose_is_called()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");

			await Session.Save(toggle);
			Session.Dispose();

			var events = await ReadEvents(toggle.ID);

			events.ShouldBe(new[]
			{
				typeof(Event<ToggleCreated>),
				typeof(Event<TagAdded>)
			});
		}

		[Fact]
		public async Task When_commit_is_called_twice()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");

			await Session.Save(toggle);
			await Session.Commit();

			var before = await ReadEvents(toggle.ID);
			before.Count().ShouldBe(2);

			await Session.Commit();

			var after = await ReadEvents(toggle.ID);
			after.Count().ShouldBe(2);
		}

		[Fact]
		public async Task When_commit_is_called_after_abort()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");

			await Session.Save(toggle);
			await Session.Abort();

			await Session.Commit();

			var events = await ReadEvents(toggle.ID);
			events.ShouldBeEmpty();
		}

		[Fact]
		public void When_there_is_no_projection_registered()
		{
			Should.Throw<ProjectionNotRegisteredException>(() => Session.QueryProjection<ToggleView>());
		}

		[Fact]
		public async Task When_the_projection_hasnt_been_written_to()
		{
			AddDefaultProjection();

			var projection = await Session.QueryProjection<ToggleView>();

			projection.ShouldBeEmpty();
		}

		[Fact]
		public async Task When_there_is_a_projection()
		{
			AddDefaultProjection();

			var toggle = Toggle.CreateNew(Editor, "Projected", "yes");

			await Session.Save(toggle);
			await Session.Commit();

			var diskProjection = await ReadProjection<ToggleView>();

			diskProjection.Select(t => t.ID).ShouldBe(new[] { toggle.ID });
		}

		[Fact]
		public async Task When_retrieving_a_projection_from_disk()
		{
			AddDefaultProjection();

			var toggle = Toggle.CreateNew(Editor, "Projected", "yes");

			await Session.Save(toggle);
			await Session.Commit();

			var projection = await Session.QueryProjection<ToggleView>();

			projection.ShouldHaveSingleItem().ID.ShouldBe(toggle.ID);
		}

		[Fact]
		public async Task Two_different_aggregate_types_can_have_the_same_id_and_not_see_each_others_events()
		{
			Builders[typeof(FirstAggregate)] = events => AggregateBuilder.Build(new FirstAggregate(), events);
			Builders[typeof(SecondAggregate)] = events => AggregateBuilder.Build(new SecondAggregate(), events);

			var id = Guid.NewGuid();

			var first = new FirstAggregate();
			first.SetID(id);
			first.AddEvents();

			var second = new SecondAggregate();
			second.SetID(id);
			second.AddEvents();

			await Session.Save(first);
			await Session.Save(second);
			await Session.Commit();

			var one = await Session.LoadAggregate<FirstAggregate>(id);
			var two = await Session.LoadAggregate<SecondAggregate>(id);

			one.SeenEvents.ShouldBe(5);
			two.SeenEvents.ShouldBe(5);
		}

		public async Task DisposeAsync() => await Task.CompletedTask;


		private class FirstAggregate : AggregateRoot<Guid>
		{
			public int SeenEvents { get; set; }

			public void SetID(Guid id) => ID = id;
			public void AddEvents() => Enumerable.Range(0, 5).Each(i => ApplyEvent(new CountEvent { Count = i }));
			public void Apply(CountEvent @event) => SeenEvents += 1;
		}

		private class SecondAggregate : AggregateRoot<Guid>
		{
			public int SeenEvents { get; set; }

			public void SetID(Guid id) => ID = id;
			public void AddEvents() => Enumerable.Range(0, 5).Each(i => ApplyEvent(new CountEvent { Count = i }));
			public void Apply(CountEvent @event) => SeenEvents += 1;
		}

		private class CountEvent
		{
			public int Count { get; set; }
		}
	}
}
