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
		protected Dictionary<Type, Func<IEnumerable<IAct>, AggregateRoot>> Builders { get; }
		protected List<Projector> Projections { get; }
		protected EditorID Editor { get; }

		protected StorageSessionTests()
		{
			Builders = new Dictionary<Type, Func<IEnumerable<IAct>, AggregateRoot>>();
			Projections = new List<Projector>();
			Editor = EditorID.Parse("wat");

			Builders[typeof(Toggle)] = events => AggregateBuilder.Build(new Toggle(), events);
		}

		public async Task InitializeAsync() => Session = await CreateSession();

		protected abstract Task<IStorageSession> CreateSession();

		protected abstract Task<bool> AggregateExists(ToggleID toggleID);
		protected abstract Task WriteEvents(ToggleID toggleID, params IAct[] events);
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
				new ToggleCreated(Editor, toggleID, "First", "hi").AsAct(toggleID),
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
				typeof(Act<ToggleCreated>),
				typeof(Act<TagAdded>),
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
				typeof(Act<ToggleCreated>),
				typeof(Act<TagAdded>)
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

		public async Task DisposeAsync() => await Task.CompletedTask;
	}
}
