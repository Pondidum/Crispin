using System.Collections.Generic;
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
			_aggregate.GetPendingEvents().ShouldBeEmpty();
		}

		[Fact]
		public void When_an_event_is_applied()
		{
			var testEvent = new TestEventOne();
			_aggregate.Raise(testEvent);

			_aggregate.GetPendingEvents().ShouldHaveSingleItem().ShouldBe(testEvent);
		}

		[Fact]
		public void When_an_applied_event_has_been_saved()
		{
			_aggregate.Raise(new TestEventOne());
			_aggregate.ClearPendingEvents();

			_aggregate.GetPendingEvents().ShouldBeEmpty();
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

			_aggregate.GetPendingEvents().ShouldBe(events);
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
			_aggregate.LoadFromEvents(events);

			_aggregate.ShouldSatisfyAllConditions(
				() => _aggregate.GetPendingEvents().ShouldBeEmpty(),
				() => _aggregate.SeenEvents.ShouldBe(events)
			);
		}

		private class TestAggregate : AggregateRoot
		{
			public List<object> SeenEvents { get; private set; }

			public TestAggregate()
			{
				SeenEvents = new List<object>();
				Register<TestEventOne>(SeenEvents.Add);
			}

			public void Raise(object e) => ApplyEvent(e);
		}

		private class TestEventOne {}
	}
}
