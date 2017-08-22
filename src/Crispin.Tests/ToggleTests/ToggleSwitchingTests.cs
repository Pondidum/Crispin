using System;
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

			Toggle.SwitchOn("user-1");

			Toggle.IsActive(Membership, "user-1").ShouldBe(true);
			Events.ShouldBe(new[]
			{
				typeof(ToggleSwitchedOn)
			});
		}

		[Fact]
		public void When_an_on_toggle_is_turned_off()
		{
			CreateToggle(new ToggleSwitchedOn());

			Toggle.SwitchOff();

			Toggle.IsActive(Membership, "").ShouldBe(false);
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
			CreateToggle(new ToggleSwitchedOn { TimeStamp = now });

			Toggle.LastToggled.ShouldBe(now);
		}

		[Fact]
		public void When_switch_off()
		{
			var now = DateTime.Now.AddHours(-123);
			CreateToggle(new ToggleSwitchedOff { TimeStamp = now });

			Toggle.LastToggled.ShouldBe(now);
		}

		private const string AnonymousUser = null;
		private const string User1InGroup1 = "user-1 => group-1";
		private const string User2InGroup2 = "user-2 => group-2";
		private const string User3 = "user-3";
		private const string User4 = "user-4";
		private const string User5InGroup1and2 = "user-5 => group-1, group-2";

		private const string Group1 = "group-1";
		private const string Group2 = "group-2";

		[Fact]
		public void When_when_a_toggle_is_in_the_default_state()
		{
			CreateToggle();

			var allUsers = new[] { AnonymousUser, User1InGroup1, User2InGroup2, User3, User4 };

			Toggle.IsActive(Membership, AnonymousUser).ShouldBeFalse();
			Toggle.IsActive(Membership, User1InGroup1).ShouldBeFalse();
			Toggle.IsActive(Membership, User2InGroup2).ShouldBeFalse();
			Toggle.IsActive(Membership, User3).ShouldBeFalse();
			Toggle.IsActive(Membership, User4).ShouldBeFalse();
		}

		[Fact]
		public void When_a_toggle_is_active_for_everyone()
		{
			CreateToggle(new ToggleSwitchedOn());

			Toggle.IsActive(Membership, AnonymousUser).ShouldBeTrue();
			Toggle.IsActive(Membership, User1InGroup1).ShouldBeTrue();
			Toggle.IsActive(Membership, User2InGroup2).ShouldBeTrue();
			Toggle.IsActive(Membership, User3).ShouldBeTrue();
			Toggle.IsActive(Membership, User4).ShouldBeTrue();
		}

		[Fact]
		public void When_a_toggle_is_active_for_a_user()
		{
			CreateToggle(new ToggleSwitchedOn(user: User3));

			Toggle.IsActive(Membership, AnonymousUser).ShouldBeFalse();
			Toggle.IsActive(Membership, User1InGroup1).ShouldBeFalse();
			Toggle.IsActive(Membership, User2InGroup2).ShouldBeFalse();
			Toggle.IsActive(Membership, User3).ShouldBe(true);
			Toggle.IsActive(Membership, User4).ShouldBeFalse();
		}

		[Fact]
		public void When_a_toggle_is_active_for_a_group()
		{
			CreateToggle(new ToggleSwitchedOn(group: Group2));

			Membership.GetGroupsFor(User1InGroup1).Returns(new[] { Group1 });
			Membership.GetGroupsFor(User2InGroup2).Returns(new[] { Group2 });

			Toggle.IsActive(Membership, AnonymousUser).ShouldBeFalse();
			Toggle.IsActive(Membership, User1InGroup1).ShouldBeFalse();
			Toggle.IsActive(Membership, User2InGroup2).ShouldBe(true);
			Toggle.IsActive(Membership, User3).ShouldBeFalse();
			Toggle.IsActive(Membership, User4).ShouldBeFalse();
		}

		[Fact]
		public void When_a_toggle_is_active_for_one_group_and_inactive_for_another()
		{
			CreateToggle(
				new ToggleSwitchedOn(group: Group1),
				new ToggleSwitchedOff(group: Group2));

			Membership.GetGroupsFor(User1InGroup1).Returns(new[] { Group1 });
			Membership.GetGroupsFor(User2InGroup2).Returns(new[] { Group2 });

			Toggle.IsActive(Membership, AnonymousUser).ShouldBeFalse();
			Toggle.IsActive(Membership, User1InGroup1).ShouldBe(true);
			Toggle.IsActive(Membership, User2InGroup2).ShouldBe(false);
			Toggle.IsActive(Membership, User3).ShouldBeFalse();
			Toggle.IsActive(Membership, User4).ShouldBeFalse();
			Toggle.IsActive(Membership, User5InGroup1and2).ShouldBe(false);
		}
	}
}
