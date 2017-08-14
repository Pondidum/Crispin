using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class InMemorySessionTests
	{
		private InMemorySession _session;
		private Dictionary<Type, Func<List<Event>, AggregateRoot>> _builders;
		private Dictionary<Guid, List<Event>> _eventStore;
		private Guid _aggregateID;

		public InMemorySessionTests()
		{
			_aggregateID = Guid.NewGuid();
			_builders = new Dictionary<Type, Func<List<Event>, AggregateRoot>>
			{
				{ typeof(Toggle), e => Toggle.LoadFrom(() => "", e) }
			};

			_eventStore = new Dictionary<Guid, List<Event>>();
			_session = new InMemorySession(_builders, _eventStore);
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
				new ToggleSwitchedOn()
			};

			var toggle = _session.LoadAggregate<Toggle>(_aggregateID);

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(_aggregateID),
				() => toggle.Active.ShouldBe(true),
				() => toggle.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public void When_saving_an_aggregate()
		{
			var toggle = Toggle.CreateNew(() => "", "First", "hi");
			toggle.AddTag("one");
			toggle.SwitchOn();

			_session.Save(toggle);

			_eventStore[toggle.ID].Select(e => e.GetType()).ShouldBe(new []
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(ToggleSwitchedOn)
			});
		}
	}
}
