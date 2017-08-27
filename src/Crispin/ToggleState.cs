using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Crispin
{
	public class ToggleState
	{
		private readonly Dictionary<UserID, bool> _users;
		private readonly Dictionary<GroupID, bool> _groups;
		private bool _anonymousActive;

		public ToggleState()
		{
			_users = new Dictionary<UserID, bool>();
			_groups = new Dictionary<GroupID, bool>();
			_anonymousActive = false;
		}

		public bool AnonymousState => _anonymousActive;
		public Dictionary<UserID, bool> UserState => new Dictionary<UserID, bool>(_users);
		public Dictionary<GroupID, bool> GroupState => new Dictionary<GroupID, bool>(_groups);

		public void HandleSwitching(UserID user, bool active)
		{
			var hasUser = user != UserID.Empty;
			if (hasUser)
				_users[user] = active;
		}

		public void HandleSwitching(GroupID group, bool active)
		{
			var hasGroup = group != GroupID.Empty;
			if (hasGroup)
				_groups[group] = active;
		}

		public void HandleSwitching(bool active)
		{
			_anonymousActive = active;
		}

		public bool IsActive(IGroupMembership membership, UserID userID)
		{
			if (userID == UserID.Empty)
				return _anonymousActive;

			if (_users.ContainsKey(userID))
				return _users[userID];

			var userGroups = membership.GetGroupsFor(userID);

			var setGroups = userGroups
				.Where(g => _groups.ContainsKey(g))
				.Select(g => _groups[g])
				.ToArray();

			if (setGroups.Any() && setGroups.All(x => x))
			{
				return true;
			}

			return _anonymousActive;
		}
	}
}
