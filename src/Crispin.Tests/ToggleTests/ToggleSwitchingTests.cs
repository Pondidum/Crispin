using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleSwitchingTests
	{
		private Toggle _toggle;

		public void CreateToggle(params object[] events)
		{
			var create = new ToggleCreated(Guid.NewGuid(), "Test Toggle", "");

			_toggle = Toggle.LoadFrom(
				() => string.Empty,
				new[] { create }.Concat(events));
		}

		private IEnumerable<object> Events => ((IEvented)_toggle).GetPendingEvents().Select(e => e.GetType());

		[Fact]
		public void When_nothing_has_happened()
		{
			CreateToggle();
			Events.ShouldBeEmpty();
		}

		[Fact]
		public void When_an_off_toggle_is_turned_on()
		{
			CreateToggle();

			_toggle.SwitchOn();

			_toggle.Active.ShouldBe(true);
			Events.ShouldBe(new[]
			{
				typeof(ToggleSwitchedOn)
			});
		}

		[Fact]
		public void When_an_on_toggle_is_turned_on()
		{
			CreateToggle(new ToggleSwitchedOn());

			_toggle.SwitchOn();

			_toggle.Active.ShouldBe(true);
			Events.ShouldBeEmpty();
		}

		[Fact]
		public void When_an_on_toggle_is_turned_off()
		{
			CreateToggle(new ToggleSwitchedOn());

			_toggle.SwitchOff();

			_toggle.Active.ShouldBe(false);
			Events.ShouldBe(new[]
			{
				typeof(ToggleSwitchedOff)
			});
		}

		[Fact]
		public void When_an_off_toggle_is_turned_off()
		{
			CreateToggle(new ToggleSwitchedOff());

			_toggle.SwitchOff();

			_toggle.Active.ShouldBe(false);
			Events.ShouldBeEmpty();
		}

		[Fact]
		public void When_a_toggle_has_not_been_switched()
		{
			CreateToggle();

			_toggle.LastToggled.HasValue.ShouldBe(false);
		}

		[Fact]
		public void When_switching_on()
		{
			var now = DateTime.Now.AddHours(-123);
			CreateToggle(new ToggleSwitchedOn { TimeStamp = now });

			_toggle.LastToggled.ShouldBe(now);
		}

		[Fact]
		public void When_switch_off()
		{
			var now = DateTime.Now.AddHours(-123);
			CreateToggle(new ToggleSwitchedOff { TimeStamp = now });

			_toggle.LastToggled.ShouldBe(now);
		}
	}
}
