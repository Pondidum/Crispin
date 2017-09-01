using System.Collections.Generic;
using System.Linq;

namespace Crispin
{
	public class ToggleState
	{
		private readonly Dictionary<UserID, State> _users;
		private readonly Dictionary<GroupID, State> _groups;
		private State _anonymousActive;

		public ToggleState()
		{
			_users = new Dictionary<UserID, State>();
			_groups = new Dictionary<GroupID, State>();
			_anonymousActive = State.Off;
		}

		public State? AnonymousState => _anonymousActive;
		public Dictionary<UserID, State> UserState => new Dictionary<UserID, State>(_users);
		public Dictionary<GroupID, State> GroupState => new Dictionary<GroupID, State>(_groups);

		public void HandleSwitching(UserID user, State? newState)
		{
			var hasUser = user != UserID.Empty;
			if (hasUser && newState.HasValue)
				_users[user] = newState.Value;
		}

		public void HandleSwitching(GroupID group, State? newState)
		{
			var hasGroup = group != GroupID.Empty;
			if (hasGroup && newState.HasValue)
				_groups[group] = newState.Value;
		}

		public void HandleSwitching(State newState)
		{
			_anonymousActive = newState;
		}

		public bool IsActive(IGroupMembership membership, UserID userID)
		{
			if (userID == UserID.Empty)
				return _anonymousActive == State.On;

			if (_users.ContainsKey(userID))
				return _users[userID]  == State.On;

			var userGroups = membership.GetGroupsFor(userID);

			var setGroups = userGroups
				.Where(g => _groups.ContainsKey(g))
				.Select(g => _groups[g])
				.ToArray();

			if (setGroups.Any() && setGroups.All(x => x == State.On))
			{
				return true;
			}

			return _anonymousActive == State.On;
		}
	}
}
