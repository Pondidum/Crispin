using System;
using System.Collections.Generic;
using Crispin.Events;
using Shouldly;
using System.Linq;
using NSubstitute;
using Xunit;

namespace Crispin.Tests
{
	public class ToggleStateTests
	{
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
				var noSwitches = Array.Empty<State>();
				var onFor3 = new State[] { new State { User = User3, Active = true }};
				var onForGroup2 = new State[] { new State { Group = Group2, Active = true}};
				var overlappingGroups = new[]
				{
					new State { Group = Group1, Active = true },
					new State { Group = Group2, Active = false }
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
		public void When_testing_all_toggly_things(State[] events, string user, bool expected)
		{
			var state = new ToggleState();
			foreach (var e in events)
				state.HandleSwitching(e.User, e.Group, e.Active);

			var membership = Substitute.For<IGroupMembership>();
			membership.GetGroupsFor(User1InGroup1).Returns(new[] { Group1 });
			membership.GetGroupsFor(User2InGroup2).Returns(new[] { Group2 });

			state.IsActive(membership, user).ShouldBe(expected);
		}

		public class State
		{
			public string User { get; set; }
			public string Group { get; set; }
			public bool Active { get; set; }
		}
	}
}
