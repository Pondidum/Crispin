using System.Collections.Generic;
using System.Linq;

namespace Crispin
{
	public class ToggleState
	{
		private readonly Dictionary<UserID, States> _users;
		private readonly Dictionary<GroupID, States> _groups;
		private States _anonymousActive;

		public ToggleState()
		{
			_users = new Dictionary<UserID, States>();
			_groups = new Dictionary<GroupID, States>();
			_anonymousActive = States.Off;
		}

		public States AnonymousState => _anonymousActive;
		public Dictionary<UserID, States> UserState => new Dictionary<UserID, States>(_users);
		public Dictionary<GroupID, States> GroupState => new Dictionary<GroupID, States>(_groups);

		public void HandleSwitching(UserID user, States? newState)
		{
			var hasUser = user != UserID.Empty;
			var hasValue = newState.HasValue;

			if (!hasUser)
				return;

			if (hasValue)
				_users[user] = newState.Value;
			else
				_users.Remove(user);
		}

		public void HandleSwitching(GroupID group, States? newState)
		{
			var hasGroup = group != GroupID.Empty;
			if (hasGroup && newState.HasValue)
				_groups[group] = newState.Value;
		}

		public void HandleSwitching(States newState)
		{
			_anonymousActive = newState;
		}

		public bool IsActive(IGroupMembership membership, UserID userID)
		{
			if (userID == UserID.Empty)
				return _anonymousActive == States.On;

			if (_users.ContainsKey(userID))
				return _users[userID]  == States.On;

			var userGroups = membership.GetGroupsFor(userID);

			var setGroups = userGroups
				.Where(g => _groups.ContainsKey(g))
				.Select(g => _groups[g])
				.ToArray();

			if (setGroups.Any() && setGroups.All(x => x == States.On))
			{
				return true;
			}

			return _anonymousActive == States.On;
		}
	}
}
