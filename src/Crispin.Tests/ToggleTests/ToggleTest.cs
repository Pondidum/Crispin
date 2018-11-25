using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;

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

		protected void CreateToggle(params IEvent[] events)
		{
			var create = new ToggleCreated(
				Editor,
				ToggleID.CreateNew(),
				"Test Toggle",
				"some description",
				ConditionModes.All);

			Toggle = new Toggle();

			AggregateBuilder.Build(Toggle, new[] { create.AsAct(create.NewToggleID) }.Concat(events));
		}

		protected void CreateToggle(Action<Toggle> setup)
		{
			CreateToggle();
			setup(Toggle);
			((IEvented)Toggle).ClearPendingEvents();
		}

		protected IEnumerable<Type> EventTypes => Events.Select(e => e.GetType());
		protected IEvent[] Events => ((IEvented)Toggle).GetPendingEvents().ToArray();

		protected Event<TEvent> Event<TEvent>(int index) => Events.Skip(index).Cast<Event<TEvent>>().First();

		protected TEvent SingleEvent<TEvent>() => ((IEvented)Toggle).GetPendingEvents().Select(e => e.Data).Cast<TEvent>().Single();
		protected void SingleEvent<TEvent>(Action<TEvent> callback) => callback(SingleEvent<TEvent>());
	}
}
