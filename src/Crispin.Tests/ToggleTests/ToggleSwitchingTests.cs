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

		public static IEnumerable<object[]> ToggleStateMatrix
		{
			get
			{
				var noSwitches = Array.Empty<object>();
				var onFor3 = new object[] { new ToggleSwitchedOn(user: User3) };
				var onForGroup2 = new object[] { new ToggleSwitchedOn(group: Group2) };
				var overlappingGroups = new object[] {
					new ToggleSwitchedOn(group: Group1),
					new ToggleSwitchedOff(group: Group2)
				};

				var matrix = new object[][]
				{
					new object[] { noSwitches, AnonymousUser, false },
					new object[] { noSwitches, User1InGroup1, false },
					new object[] { noSwitches, User2InGroup2, false },
					new object[] { noSwitches, User3, false },
					new object[] { noSwitches, User4, false },
					new object[] { noSwitches, User5InGroup1and2, false },
					new object[] { onFor3, AnonymousUser, false },
					new object[] { onFor3, User1InGroup1, false },
					new object[] { onFor3, User2InGroup2, false },
					new object[] { onFor3, User3, true },
					new object[] { onFor3, User4, false },
					new object[] { onFor3, User5InGroup1and2, false },
					new object[] { onForGroup2, AnonymousUser, false },
					new object[] { onForGroup2, User1InGroup1, false },
					new object[] { onForGroup2, User2InGroup2, true },
					new object[] { onForGroup2, User3, false },
					new object[] { onForGroup2, User4, false },
					new object[] { onForGroup2, User5InGroup1and2, false },
					new object[] { overlappingGroups, AnonymousUser, false },
					new object[] { overlappingGroups, User1InGroup1, true },
					new object[] { overlappingGroups, User2InGroup2, false },
					new object[] { overlappingGroups, User3, false },
					new object[] { overlappingGroups, User4, false },
					new object[] { overlappingGroups, User5InGroup1and2, false }
				};

				return matrix.AsEnumerable();
			}
		}

		[Theory]
		[MemberData(nameof(ToggleStateMatrix))]
		public void When_testing_all_toggly_things(object[] events, string user, bool expected)
		{
			CreateToggle(events);
			Membership.GetGroupsFor(User1InGroup1).Returns(new[] { Group1 });
			Membership.GetGroupsFor(User2InGroup2).Returns(new[] { Group2 });

			Toggle.IsActive(Membership, user).ShouldBe(expected);
		}
	}
}
