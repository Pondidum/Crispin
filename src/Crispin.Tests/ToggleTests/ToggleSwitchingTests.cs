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
				typeof(ToggleSwitchedOnForUser)
			});
		}

		[Fact]
		public void When_an_on_toggle_is_turned_off()
		{
			CreateToggle(new ToggleSwitchedOnForAnonymous());

			Toggle.SwitchOffByDefault();

			Toggle.IsActive(Membership, UserID.Empty).ShouldBe(false);
			Events.ShouldBe(new[]
			{
				typeof(ToggleSwitchedOffForAnonymous)
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
			CreateToggle(new ToggleSwitchedOnForAnonymous() { TimeStamp = now });

			Toggle.LastToggled.ShouldBe(now);
		}

		[Fact]
		public void When_switch_off()
		{
			var now = DateTime.Now.AddHours(-123);
			CreateToggle(new ToggleSwitchedOffForAnonymous() { TimeStamp = now });

			Toggle.LastToggled.ShouldBe(now);
		}
	}
}
