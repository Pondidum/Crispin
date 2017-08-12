using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Infrastructure;
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

			((IEvented)_aggregate).GetPendingEvents().ShouldHaveSingleItem().ShouldBe(testEvent);
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

			((IEvented)_aggregate).GetPendingEvents().ShouldBe(events);
		}

		[Fact]
		public void When_loading_from_events()
		{
			var events = new[]
			{
				new TestEventOne(),
				new TestEventOne(),
				new TestEventOne()
			};
			((IEvented)_aggregate).LoadFromEvents(events);

			_aggregate.ShouldSatisfyAllConditions(
				() => ((IEvented)_aggregate).GetPendingEvents().ShouldBeEmpty(),
				() => _aggregate.SeenEvents.ShouldBe(events)
			);
		}

		[Fact]
		public void When_a_timestampable_event_is_applied()
		{
			_aggregate.Raise(new Stamped());

			_aggregate
				.SeenEvents
				.Cast<Stamped>()
				.Single()
				.TimeStamp
				.ShouldNotBeNull();
		}

		[Fact]
		public void When_a_timestampable_event_is_loaded()
		{
			var stamp = new DateTime(2017, 2, 3);

			_aggregate.LoadFrom(new Stamped { TimeStamp = stamp });

			_aggregate.SeenEvents.Cast<Stamped>().Single().TimeStamp.ShouldBe(stamp);
		}

		[Fact]
		public void When_applying_an_event()
		{
			var id = Guid.NewGuid();
			_aggregate.Raise(new TestAggregateCreated(id));
			_aggregate.Raise(new TestEventOne());

			_aggregate.SeenEvents.Last().AggregateID.ShouldBe(id);
		}

		[Fact]
		public void When_populating_extra_event_data()
		{
			var userID = Guid.NewGuid().ToString();
			var aggregate = new CrossCuttingAggregate(() => userID);
			aggregate.Raise(new TestEventOne());

			aggregate.SeenEvents.Single().UserID.ShouldBe(userID);
		}

		private class CrossCuttingAggregate : AggregateRoot
		{
			private readonly Func<string> _extra;
			public List<Event> SeenEvents { get; private set; }

			public CrossCuttingAggregate(Func<string> extra)
			{
				_extra = extra;
				SeenEvents = new List<Event>();
				Register<TestEventOne>(SeenEvents.Add);
				Register<Stamped>(SeenEvents.Add);
			}

			protected override void PopulateExtraEventData(Event @event)
			{
				base.PopulateExtraEventData(@event);
				@event.UserID = _extra();
			}

			public void Raise(Event e) => ApplyEvent(e);
		}

		private class TestAggregate : AggregateRoot
		{
			public List<Event> SeenEvents { get; private set; }

			public TestAggregate()
			{
				SeenEvents = new List<Event>();
				Register<TestEventOne>(SeenEvents.Add);
				Register<Stamped>(SeenEvents.Add);
				Register<TestAggregateCreated>(e =>
				{
					ID = e.ID;
					SeenEvents.Add(e);
				});
			}

			public void LoadFrom(params object[] events)
			{
				((IEvented)this).LoadFromEvents(events);
			}

			public void Raise(Event e) => ApplyEvent(e);
		}

		private class TestEventOne : Event {}
		private class Stamped : Event {}

		private class TestAggregateCreated : Event
		{
			public Guid ID { get; }

			public TestAggregateCreated(Guid id)
			{
				ID = id;
			}
		}
	}
}
