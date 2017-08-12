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

		protected void CreateToggle(params object[] events)
		{
			var create = new ToggleCreated(Guid.NewGuid(), "Test Toggle", "");

			Toggle = Toggle.LoadFrom(
				() => string.Empty,
				new[] { create }.Concat(events));
		}

		protected IEnumerable<object> Events => ((IEvented)Toggle).GetPendingEvents().Select(e => e.GetType());
		protected TEvent SingleEvent<TEvent>() => (TEvent)((IEvented)Toggle).GetPendingEvents().Single();
	}
}
