using System.Collections.Generic;
using Crispin.Infrastructure;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Crispin.Tests.Infrastructure
{
	public class ProjectionTests
	{
		[Fact]
		public void When_consuming_all_events()
		{
			var first = new FirstEvent();
			var second = new SecondEvent();

			var projection = new CatchAllProjection();
			projection.Consume(first);
			projection.Consume(second);

			projection.SeenEvents.ShouldBe(new Event[]
			{
				first,
				second
			});
		}

		[Fact]
		public void When_consuming_specific_events()
		{
			var first = new FirstEvent();
			var second = new SecondEvent();

			var projection = new TestProjection();
			projection.Consume(first);
			projection.Consume(second);
			projection.Consume(new ThirdEvent());

			projection.SeenEvents.ShouldBe(new Event[]
			{
				first,
				second
			});
		}

		public class FirstEvent : Event {}
		public class SecondEvent : Event {}
		public class ThirdEvent : Event {}

		private class CatchAllProjection : Projection<Memento>
		{
			public List<Event> SeenEvents { get; }

			public CatchAllProjection()
			{
				SeenEvents = new List<Event>();
				RegisterAll(SeenEvents.Add);
			}

			protected override Memento CreateMemento()
			{
				return new Memento();
			}

			protected override void ApplyMemento(Memento memento)
			{
			}
		}

		private class TestProjection : Projection<Memento>
		{
			public List<Event> SeenEvents { get; }

			public TestProjection()
			{
				SeenEvents = new List<Event>();
				Register<FirstEvent>(SeenEvents.Add);
				Register<SecondEvent>(SeenEvents.Add);
			}

			protected override Memento CreateMemento()
			{
				return new Memento();
			}

			protected override void ApplyMemento(Memento memento)
			{
			}
		}
	}

	internal class Memento
	{
	}
}
