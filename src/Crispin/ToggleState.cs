using System;
using System.Collections.Generic;
using System.Linq;

namespace Crispin
{
	public class ToggleState
	{
		private readonly Dictionary<string, bool> _users;
		private readonly Dictionary<string, bool> _groups;
		private bool _anonymousActive;

		public ToggleState()
		{
			_users = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
			_groups = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
			_anonymousActive = false;
		}

		public bool AnonymousState => _anonymousActive;
		public Dictionary<string, bool> UserState => new Dictionary<string, bool>(_users);
		public Dictionary<string, bool> GroupState => new Dictionary<string, bool>(_groups);
		
		public void HandleSwitching(string user, string @group, bool active)
		{
			var hasUser = string.IsNullOrWhiteSpace(user) == false;
			var hasGroup = string.IsNullOrWhiteSpace(group) == false;

			if (hasUser)
				_users[user] = active;

			if (hasGroup)
				_groups[group] = active;

			if (hasUser == false && hasGroup == false)
				_anonymousActive = active;
		}

		public bool IsActive(IGroupMembership membership, string userID)
		{
			if (string.IsNullOrWhiteSpace(userID))
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
