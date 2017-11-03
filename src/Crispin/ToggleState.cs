using System;
using System.Linq;
using Crispin.Projections;

namespace Crispin
{
	public class ToggleState
	{
		private readonly StateView _view;

		public ToggleState() : this(new StateView())
		{
		}

		public ToggleState(StateView view)
		{
			_view = view ?? throw new ArgumentNullException(nameof(view));
		}

		public void HandleSwitching(UserID user, States? newState)
		{
			var hasUser = user != UserID.Empty;
			var hasValue = newState.HasValue;

			if (hasUser == false)
				return;

			if (hasValue)
				_view.Users[user] = newState.Value;
			else
				_view.Users.Remove(user);
		}

		public void HandleSwitching(GroupID group, States? newState)
		{
			var hasGroup = group != GroupID.Empty;
			var hasValue = newState.HasValue;

			if (hasGroup == false)
				return;

			if (hasValue)
				_view.Groups[group] = newState.Value;
			else
				_view.Groups.Remove(group);
		}

		public void HandleSwitching(States newState)
		{
			_view.Anonymous = newState;
		}

		public bool IsActive(IGroupMembership membership, UserID userID)
		{
			if (userID == UserID.Empty)
				return _view.Anonymous == States.On;

			if (_view.Users.ContainsKey(userID))
				return _view.Users[userID] == States.On;

			var userGroups = membership.GetGroupsFor(userID);

			var setGroups = userGroups
				.Where(g => _view.Groups.ContainsKey(g))
				.Select(g => _view.Groups[g])
				.ToArray();

			if (setGroups.Any() && setGroups.All(x => x == States.On))
			{
				return true;
			}

			return _view.Anonymous == States.On;
		}
	}
}
