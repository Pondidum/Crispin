using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;
using Microsoft.VisualBasic.CompilerServices;
using NSubstitute;

namespace Crispin.Tests.ToggleTests
{
	public abstract class ToggleTest
	{
		protected Toggle Toggle { get; set; }
		protected IGroupMembership Membership { get; }
		protected EditorID Editor { get; }

		protected ToggleTest()
		{
			Membership = Substitute.For<IGroupMembership>();
			Editor = EditorID.Parse("Testing");
		}

		protected void CreateToggle(params object[] events)
		{
			var create = new ToggleCreated(
				Editor,
				ToggleID.CreateNew(),
				"Test Toggle",
				"");

			Toggle = Toggle.LoadFrom(
				new[] { create }.Concat(events));
		}

		protected IEnumerable<object> Events => ((IEvented)Toggle).GetPendingEvents().Select(e => e.GetType());
		protected TEvent SingleEvent<TEvent>() => (TEvent)((IEvented)Toggle).GetPendingEvents().Single();
	}
}
