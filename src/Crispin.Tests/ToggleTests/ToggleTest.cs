using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;

namespace Crispin.Tests.ToggleTests
{
	public abstract class ToggleTest
	{
		protected Toggle Toggle { get; set; }
		protected EditorID Editor { get; }

		protected ToggleTest()
		{
			Editor = EditorID.Parse("Testing");
		}

		protected void CreateToggle(params Event[] events)
		{
			var create = new ToggleCreated(
				Editor,
				ToggleID.CreateNew(),
				"Test Toggle",
				"");

			Toggle = new Toggle();

			var loader = new Aggregator(Toggle);
			loader.Apply(Toggle, new[] { create }.Concat(events));
		}

		protected void CreateToggle(Action<Toggle> setup)
		{
			CreateToggle();
			setup(Toggle);
			((IEvented)Toggle).ClearPendingEvents();
		}

		protected IEnumerable<Type> EventTypes => Events.Select(e => e.GetType());
		protected Event[] Events => ((IEvented)Toggle).GetPendingEvents().ToArray();

		protected TEvent Event<TEvent>(int index) => Events.Skip(index).Cast<TEvent>().First();

		protected TEvent SingleEvent<TEvent>() where TEvent : Event => (TEvent)((IEvented)Toggle).GetPendingEvents().Single();
		protected void SingleEvent<TEvent>(Action<TEvent> callback) where TEvent : Event => callback(SingleEvent<TEvent>());
	}
}
