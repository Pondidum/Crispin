using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Events;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleSwitchingTests : ToggleTest
	{
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

			Toggle.SwitchOn(UserID.Parse("user-1"));

			Toggle.IsActive(Membership, UserID.Parse("user-1")).ShouldBe(true);
			Events.ShouldBe(new[]
			{
				typeof(ToggleSwitchedOn)
			});
		}

		[Fact]
		public void When_an_on_toggle_is_turned_off()
		{
			CreateToggle(new ToggleSwitchedOn(UserID.Empty));

			Toggle.SwitchOff(UserID.Empty);

			Toggle.IsActive(Membership, UserID.Empty).ShouldBe(false);
			Events.ShouldBe(new[]
			{
				typeof(ToggleSwitchedOff)
			});
		}

		[Fact]
		public void When_a_toggle_has_not_been_switched()
		{
			CreateToggle();

			Toggle.LastToggled.HasValue.ShouldBe(false);
		}

		[Fact]
		public void When_switching_on()
		{
			var now = DateTime.Now.AddHours(-123);
			CreateToggle(new ToggleSwitchedOn(UserID.Empty) { TimeStamp = now });

			Toggle.LastToggled.ShouldBe(now);
		}

		[Fact]
		public void When_switch_off()
		{
			var now = DateTime.Now.AddHours(-123);
			CreateToggle(new ToggleSwitchedOff(UserID.Empty) { TimeStamp = now });

			Toggle.LastToggled.ShouldBe(now);
		}
	}
}
