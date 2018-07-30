using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure
{
	public class AggregatorTests
	{
		[Fact]
		public void Apply_method_with_plain_event_is_found()
		{
			var aggregator = new Aggregator(typeof(TestAggregate));
			aggregator.For<TestEvent>().ShouldNotBeNull();
		}

		[Fact]
		public void Apply_method_with_wrapped_event_is_found()
		{
			var aggregator = new Aggregator(typeof(TestAggregate));
			aggregator.For<WrappedEvent>().ShouldNotBeNull();
		}

		[Fact]
		public void Apply_method_for_unknown_event_is_null()
		{
			var aggregator = new Aggregator(typeof(TestAggregate));
			aggregator.For<UnusedEvent>().ShouldBeNull();
		}


		public class TestAggregate
		{
			public void Apply(TestEvent e)
			{
			}

			public void Apply(Event<WrappedEvent> e)
			{
			}
		}

		public class TestEvent
		{
		}

		public class WrappedEvent
		{
		}

		public class UnusedEvent
		{
		}
	}
}
