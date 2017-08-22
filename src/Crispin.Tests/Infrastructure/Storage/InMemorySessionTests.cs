using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Projections;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class InMemorySessionTests
	{
		private readonly InMemorySession _session;
		private readonly Dictionary<Type, Func<List<Event>, AggregateRoot>> _builders;
		private readonly Dictionary<Guid, List<Event>> _eventStore;
		private readonly List<Projection> _projections;
		private readonly Guid _aggregateID;
		private readonly IGroupMembership _membership;

		public InMemorySessionTests()
		{
			_aggregateID = Guid.NewGuid();
			_builders = new Dictionary<Type, Func<List<Event>, AggregateRoot>>
			{
				{ typeof(Toggle), e => Toggle.LoadFrom(() => "", e) }
			};

			_eventStore = new Dictionary<Guid, List<Event>>();
			_projections = new List<Projection>();

			_session = new InMemorySession(_builders, _projections, _eventStore);
			_membership = Substitute.For<IGroupMembership>();
		}

		[Fact]
		public void When_there_is_no_builder_for_an_aggregate()
		{
			_builders.Clear();

			Should.Throw<NotSupportedException>(() => _session.LoadAggregate<Toggle>(_aggregateID));
		}

		[Fact]
		public void When_there_is_no_aggregate_stored()
		{
			Should.Throw<KeyNotFoundException>(() => _session.LoadAggregate<Toggle>(_aggregateID));
		}

		[Fact]
		public void When_there_are_no_events_for_an_aggregate_stored()
		{
			_eventStore[_aggregateID] = new List<Event>();

			Should.Throw<KeyNotFoundException>(() => _session.LoadAggregate<Toggle>(_aggregateID));
		}

		[Fact]
		public void When_an_aggregate_is_loaded()
		{
			_eventStore[_aggregateID] = new List<Event> 
			{
				new ToggleCreated(_aggregateID, "First", "hi"),
				new TagAdded("one"),
				new ToggleSwitchedOn("user-1")
			};

			var toggle = _session.LoadAggregate<Toggle>(_aggregateID);

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(_aggregateID),
				() => toggle.IsActive(_membership, "user-1").ShouldBeTrue(),
				() => toggle.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public void When_saving_an_aggregate_and_commit_is_not_called()
		{
			var toggle = Toggle.CreateNew(() => "", "First", "hi");
			toggle.AddTag("one");
			toggle.SwitchOn();

			_session.Save(toggle);

			_eventStore.ShouldNotContainKey(toggle.ID);
		}

		[Fact]
		public void When_saving_an_aggregate_and_commit_is_called()
		{
			var toggle = Toggle.CreateNew(() => "", "First", "hi");
			toggle.AddTag("one");
			toggle.SwitchOn();

			_session.Save(toggle);
			_session.Commit();

			_eventStore[toggle.ID].Select(e => e.GetType()).ShouldBe(new []
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(ToggleSwitchedOn)
			});
		}

		[Fact]
		public void When_loading_an_aggregate_saved_in_the_current_session()
		{
			var toggle = Toggle.CreateNew(() => "", "First", "hi");
			toggle.AddTag("one");
			toggle.SwitchOn("user-1");

			_session.Save(toggle);

			var loaded = _session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.IsActive(_membership, "user-1").ShouldBe(true),
				() => loaded.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public void When_loading_an_aggregate_existing_in_store_and_saved_in_the_current_session()
		{
			var toggle = Toggle.CreateNew(() => "", "First", "hi");
			toggle.AddTag("one");

			_session.Save(toggle);
			_session.Commit();

			toggle.SwitchOn("user-1");
			_session.Save(toggle);

			var loaded = _session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.IsActive(_membership, "user-1").ShouldBe(true),
				() => loaded.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public void When_there_are_pending_events_and_dispose_is_called()
		{
			var toggle = Toggle.CreateNew(() => "", "First", "hi");
			toggle.AddTag("one");

			_session.Save(toggle);
			_session.Dispose();

			_eventStore[toggle.ID].Select(e => e.GetType()).ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded)
			});
		}

		[Fact]
		public void When_commit_is_called_twice()
		{
			var toggle = Toggle.CreateNew(() => "", "First", "hi");
			toggle.AddTag("one");

			_session.Save(toggle);
			_session.Commit();

			_eventStore[toggle.ID].Count.ShouldBe(2);
			_eventStore[toggle.ID].Clear();

			_session.Commit();
			_eventStore[toggle.ID].ShouldBeEmpty();
		}

		[Fact]
		public void When_there_is_a_projection()
		{
			var projection = new AllToggles();
			_projections.Add(projection);

			var toggle = Toggle.CreateNew(() => "", "Projected", "yes");

			_session.Save(toggle);
			_session.Commit();

			var view = projection.Toggles.Single();

			view.ShouldSatisfyAllConditions(
				() => view.ID.ShouldBe(toggle.ID),
				() => view.Name.ShouldBe(toggle.Name),
				() => view.Description.ShouldBe(toggle.Description),
				() => view.Tags.ShouldBeEmpty(),
				() => view.State.Anonymous.ShouldBeFalse(),
				() => view.State.Users.ShouldBeEmpty(),
				() => view.State.Groups.ShouldBeEmpty()
			);
		}

		[Fact]
		public void When_there_is_a_projection_with_multiple_aggregates()
		{
			var projection = new AllToggles();
			_projections.Add(projection);

			var first = Toggle.CreateNew(() => "", "First", "yes");
			var second = Toggle.CreateNew(() => "", "Second", "yes");

			_session.Save(first);
			_session.Save(second);
			_session.Commit();

			projection.Toggles.Select(v => v.ID).ShouldBe(new[]
			{
				first.ID,
				second.ID
			}, ignoreOrder: true);
		}

		[Fact]
		public void When_retrieving_a_projection_which_exists_in_the_session()
		{
			var projection = new AllToggles();
			_projections.Add(projection);

			_session.LoadProjection<AllToggles>()
				.ShouldBe(projection);
		}

		[Fact]
		public void When_retrieving_a_projection_which_doesnt_exist_in_the_session()
		{
			Should.Throw<ProjectionNotRegisteredException>(
				() => _session.LoadProjection<AllToggles>()
			);
		}
	}
}
