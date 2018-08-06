using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure
{
	public class AggregateRootTests
	{
		private readonly TestAggregate _aggregate;

		public AggregateRootTests()
		{
			_aggregate = new TestAggregate();
		}

		[Fact]
		public void When_there_are_no_events_to_be_saved()
		{
			((IEvented)_aggregate).GetPendingEvents().ShouldBeEmpty();
		}

		[Fact]
		public void When_an_event_is_applied()
		{
			var testEvent = new TestEventOne();
			_aggregate.Raise(testEvent);

			((IEvented)_aggregate).GetPendingEvents().ShouldHaveSingleItem().Data.ShouldBe(testEvent);
		}

		[Fact]
		public void When_an_applied_event_has_been_saved()
		{
			_aggregate.Raise(new TestEventOne());
			((IEvented)_aggregate).ClearPendingEvents();

			((IEvented)_aggregate).GetPendingEvents().ShouldBeEmpty();
		}

		[Fact]
		public void When_multiple_events_are_saved()
		{
			var events = new[]
			{
				new TestEventOne(),
				new TestEventOne(),
				new TestEventOne()
			};

			foreach (var e in events)
				_aggregate.Raise(e);

			((IEvented)_aggregate).GetPendingEvents().Select(e => e.Data).ShouldBe(events);
		}

		[Fact]
		public void When_loading_from_events()
		{
			var events = new IEvent[]
			{
				new TestEventOne().AsAct(),
				new TestEventOne().AsAct(),
				new TestEventOne().AsAct()
			};

			AggregateBuilder.Build(_aggregate, events);

			_aggregate.ShouldSatisfyAllConditions(
				() => ((IEvented)_aggregate).GetPendingEvents().ShouldBeEmpty(),
				() => _aggregate.SeenEvents.ShouldBe(events.Select(e => e.Data))
			);
		}

		[Fact]
		public void When_a_timestampable_event_is_applied()
		{
			_aggregate.Raise(new Stamped());

			((IEvented)_aggregate)
				.GetPendingEvents()
				.ShouldHaveSingleItem()
				.TimeStamp
				.ShouldNotBeNull();
		}

		[Fact]
		public void When_a_timestampable_event_is_loaded()
		{
			var stamp = new DateTime(2017, 2, 3);

			var act = new Event<Stamped>
			{
				AggregateID = _aggregate.ID,
				TimeStamp = stamp,
				Data = new Stamped()
			};

			AggregateBuilder.Build(_aggregate, new[] { act });

			_aggregate.LastEvent.ShouldBe(stamp);
		}

		[Fact]
		public void When_applying_an_event()
		{
			var id = Guid.NewGuid();
			_aggregate.Raise(new TestAggregateCreated(id));
			_aggregate.Raise(new TestEventOne());

			((IEvented)_aggregate)
				.GetPendingEvents()
				.ShouldAllBe(e => (Guid)e.AggregateID == id);
		}

		private class TestAggregate : AggregateRoot<Guid>
		{
			public DateTime LastEvent { get; set; }
			public List<object> SeenEvents { get; private set; }

			public TestAggregate()
			{
				SeenEvents = new List<object>();
			}

			private void Apply(TestEventOne e) => SeenEvents.Add(e);

			private void Apply(Event<Stamped> e)
			{
				LastEvent = e.TimeStamp;
				SeenEvents.Add(e.Data);
			}

			private void Apply(TestAggregateCreated e)
			{
				ID = e.ID;
				SeenEvents.Add(e);
			}

			public void Raise<TEvent>(TEvent e) => ApplyEvent(e);
		}

		private class TestEventOne
		{
		}

		private class Stamped
		{
		}

		private class TestAggregateCreated
		{
			public Guid ID { get; }

			public TestAggregateCreated(Guid id)
			{
				ID = id;
			}
		}
	}
}
