using System;
using System.Collections.Generic;
using Crispin.Events;
using Shouldly;
using System.Linq;
using Crispin.Projections;
using Crispin.Views;
using NSubstitute;
using Xunit;

namespace Crispin.Tests
{
	public class ToggleStateTests
	{
		private static readonly UserID AnonymousUser = UserID.Empty;
		private static readonly UserID User1InGroup1 = UserID.Parse("user-1 => group-1");
		private static readonly UserID User2InGroup2 = UserID.Parse("user-2 => group-2");
		private static readonly UserID User3 = UserID.Parse("user-3");
		private static readonly UserID User4 = UserID.Parse("user-4");
		private static readonly UserID User5InGroup1And2 = UserID.Parse("user-5 => group-1, group-2");

		private static readonly GroupID Group1 = GroupID.Parse("group-1");
		private static readonly GroupID Group2 = GroupID.Parse("group-2");

		public static IEnumerable<object[]> ToggleStateMatrix
		{
			get
			{
				var noSwitches = Array.Empty<State>();
				var onFor3 = new State[] { new State { User = User3, Active = States.On } };
				var onForGroup2 = new State[] { new State { Group = Group2, Active = States.On } };
				var overlappingGroups = new[]
				{
					new State { Group = Group1, Active = States.On },
					new State { Group = Group2, Active = States.Off }
				};

				var matrix = new object[][]
				{
					new object[] { noSwitches, AnonymousUser, false },
					new object[] { noSwitches, User1InGroup1, false },
					new object[] { noSwitches, User2InGroup2, false },
					new object[] { noSwitches, User3, false },
					new object[] { noSwitches, User4, false },
					new object[] { noSwitches, User5InGroup1And2, false },
					new object[] { onFor3, AnonymousUser, false },
					new object[] { onFor3, User1InGroup1, false },
					new object[] { onFor3, User2InGroup2, false },
					new object[] { onFor3, User3, true },
					new object[] { onFor3, User4, false },
					new object[] { onFor3, User5InGroup1And2, false },
					new object[] { onForGroup2, AnonymousUser, false },
					new object[] { onForGroup2, User1InGroup1, false },
					new object[] { onForGroup2, User2InGroup2, true },
					new object[] { onForGroup2, User3, false },
					new object[] { onForGroup2, User4, false },
					new object[] { onForGroup2, User5InGroup1And2, false },
					new object[] { overlappingGroups, AnonymousUser, false },
					new object[] { overlappingGroups, User1InGroup1, true },
					new object[] { overlappingGroups, User2InGroup2, false },
					new object[] { overlappingGroups, User3, false },
					new object[] { overlappingGroups, User4, false },
					new object[] { overlappingGroups, User5InGroup1And2, false }
				};

				return matrix.AsEnumerable();
			}
		}

		[Fact]
		public void The_view_if_supplied_cannot_be_null()
		{
			Should.Throw<ArgumentNullException>(() => new ToggleState(null));
		}

		[Theory]
		[MemberData(nameof(ToggleStateMatrix))]
		public void When_testing_all_toggly_things(State[] events, UserID user, bool expected)
		{
			var state = new ToggleState();
			foreach (var e in events)
			{
				if (e.User != UserID.Empty)
					state.HandleSwitching(e.User, e.Active);
				else if (e.Group != GroupID.Empty)
					state.HandleSwitching(e.Group, e.Active);
				else
					state.HandleSwitching(e.Active);
			}


			var membership = Substitute.For<IGroupMembership>();
			membership.GetGroupsFor(User1InGroup1).Returns(new[] { Group1 });
			membership.GetGroupsFor(User2InGroup2).Returns(new[] { Group2 });

			state.IsActive(membership, user).ShouldBe(expected);
		}

		[Theory]
		[InlineData(States.On, null, false, false)]
		[InlineData(States.Off, null, false, false)]
		[InlineData(States.On, States.On, true, true)]
		[InlineData(States.Off, States.On, true, true)]
		[InlineData(States.On, States.Off, true, false)]
		[InlineData(States.Off, States.Off, true, false)]
		public void When_passing_state_to_a_user(States initialState, States? newState, bool shouldContain, bool expectedActive)
		{
			var view = new StateView();
			var state = new ToggleState(view);
			state.HandleSwitching(User3, initialState);

			state.HandleSwitching(User3, newState);

			if (shouldContain)
				view.Users.ShouldContainKey(User3);
			else
				view.Users.ShouldNotContainKey(User3);

			var membership = Substitute.For<IGroupMembership>();
			membership.GetGroupsFor(Arg.Any<UserID>()).Returns(Enumerable.Empty<GroupID>());

			state.IsActive(membership, User3).ShouldBe(expectedActive);
		}

		[Theory]
		[InlineData(States.On, null, false, false)]
		[InlineData(States.Off, null, false, false)]
		[InlineData(States.On, States.On, true, true)]
		[InlineData(States.Off, States.On, true, true)]
		[InlineData(States.On, States.Off, true, false)]
		[InlineData(States.Off, States.Off, true, false)]
		public void When_passing_state_to_a_group(States initialState, States? newState, bool shouldContain, bool expectedActive)
		{
			var view = new StateView();
			var state = new ToggleState(view);
			state.HandleSwitching(Group1, initialState);

			state.HandleSwitching(Group1, newState);

			if (shouldContain)
				view.Groups.ShouldContainKey(Group1);
			else
				view.Groups.ShouldNotContainKey(Group1);

			var membership = Substitute.For<IGroupMembership>();
			membership.GetGroupsFor(Arg.Any<UserID>()).Returns(new[] { Group1 });

			state.IsActive(membership, User1InGroup1).ShouldBe(expectedActive);
		}

		public class State
		{
			public UserID User { get; set; }
			public GroupID Group { get; set; }
			public States Active { get; set; }
		}
	}
}
