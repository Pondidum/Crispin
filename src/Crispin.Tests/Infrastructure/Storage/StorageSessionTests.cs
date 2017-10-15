using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public abstract class StorageSessionTests
	{
		protected IStorageSession Session { get; set; }
		protected Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> Builders { get; }
		protected List<IProjection> Projections { get; }
		protected EditorID Editor { get; }

		protected StorageSessionTests()
		{
			Builders = new Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>>
			{
				{ typeof(Toggle), Toggle.LoadFrom }
			};
			Projections = new List<IProjection>();

			Editor = EditorID.Parse("wat");
		}

		protected abstract Task<bool> AggregateExists(ToggleID toggleID);
		protected abstract Task WriteEvents(ToggleID toggleID, params object[] events);
		protected abstract Task<IEnumerable<Type>> ReadEvents(ToggleID toggleID);
		protected abstract Task<TProjection> ReadProjection<TProjection>(TProjection projection) where TProjection : IProjection;

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
				new ToggleCreated(Editor, toggleID, "First", "hi"),
				new TagAdded(Editor, "one"),
				new ToggleSwitchedOnForUser(Editor, UserID.Parse("user-1"))
			);

			var toggle = await Session.LoadAggregate<Toggle>(toggleID);

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(toggleID),
				() => toggle.IsActive(Substitute.For<IGroupMembership>(), UserID.Parse("user-1")).ShouldBeTrue(),
				() => toggle.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public async Task When_saving_an_aggregate_and_commit_is_not_called()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");
			toggle.ChangeDefaultState(Editor, newState: States.On);

			await Session.Save(toggle);

			var exists = await AggregateExists(toggle.ID);

			exists.ShouldBeFalse();
		}

		[Fact]
		public async Task When_saving_an_aggregate_and_commit_is_called()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");
			toggle.ChangeDefaultState(Editor, newState: States.On);

			await Session.Save(toggle);
			await Session.Commit();

			var events = await ReadEvents(toggle.ID);

			events.ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(ToggleSwitchedOnForAnonymous)
			});
		}

		[Fact]
		public async Task When_loading_an_aggregate_saved_in_the_current_session()
		{
			var toggle = Toggle.CreateNew(Editor, "First", "hi");
			toggle.AddTag(Editor, "one");
			toggle.ChangeState(Editor, UserID.Parse("user-1"), States.On);

			await Session.Save(toggle);

			var loaded = await Session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.IsActive(Substitute.For<IGroupMembership>(), UserID.Parse("user-1")).ShouldBe(true),
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

			toggle.ChangeState(Editor, UserID.Parse("user-1"), States.On);
			await Session.Save(toggle);

			var loaded = await Session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.IsActive(Substitute.For<IGroupMembership>(), UserID.Parse("user-1")).ShouldBe(true),
				() => loaded.Tags.ShouldContain("one")
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
				typeof(ToggleCreated),
				typeof(TagAdded)
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
		public void When_there_is_no_projection_registered()
		{
			Should.Throw<ProjectionNotRegisteredException>(() => Session.LoadProjection<AllToggles>());
		}

		[Fact]
		public async Task When_there_is_a_projection()
		{
			var projection = new AllToggles();
			Projections.Add(projection);

			var toggle = Toggle.CreateNew(Editor, "Projected", "yes");

			await Session.Save(toggle);
			await Session.Commit();

			var view = projection.Toggles.Single();

			view.ShouldSatisfyAllConditions(
				() => view.ID.ShouldBe(toggle.ID),
				() => view.Name.ShouldBe(toggle.Name),
				() => view.Description.ShouldBe(toggle.Description),
				() => view.Tags.ShouldBeEmpty(),
				() => view.State.Anonymous.ShouldBe(States.Off),
				() => view.State.Users.ShouldBeEmpty(),
				() => view.State.Groups.ShouldBeEmpty()
			);

			var diskProjection = await ReadProjection(new AllToggles());

			diskProjection.Toggles.Select(t => t.ID).ShouldBe(projection.Toggles.Select(t => t.ID));
		}

		[Fact]
		public async Task When_retrieving_a_projection_from_disk()
		{
			Projections.Add(new AllToggles());

			var toggle = Toggle.CreateNew(Editor, "Projected", "yes");

			await Session.Save(toggle);
			await Session.Commit();

			var projection = await Session.LoadProjection<AllToggles>();

			projection.Toggles.ShouldHaveSingleItem().ID.ShouldBe(toggle.ID);
		}
	}
}
