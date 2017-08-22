using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Crispin.Events;
using Crispin.Infrastructure;

namespace Crispin
{
	public class Toggle : AggregateRoot
	{
		public static Toggle CreateNew(Func<string> getCurrentUserID, string name, string description = "")
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name), "Toggles must have a non-whitespace name.");

			var toggle = new Toggle(getCurrentUserID);
			toggle.ApplyEvent(new ToggleCreated(Guid.NewGuid(), name.Trim(), description));

			return toggle;
		}

		public static Toggle LoadFrom(Func<string> getCurrentUserID, IEnumerable<object> events)
		{
			var toggle = new Toggle(getCurrentUserID);
			((IEvented)toggle).LoadFromEvents(events);

			return toggle;
		}


		public string Name { get; private set; }
		public string Description { get; private set; }
		public DateTime? LastToggled { get; private set; }
		public IEnumerable<string> Tags => _tags;

		private readonly HashSet<string> _tags;
		private readonly Func<string> _getCurrentUserID;

		private readonly Dictionary<string, bool> _users;
		private readonly Dictionary<string, bool> _groups;
		private bool _anonymousActive;

		private Toggle(Func<string> getCurrentUserID)
		{
			_getCurrentUserID = getCurrentUserID;
			_tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			_users = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
			_groups = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
			_anonymousActive = false;

			Register<ToggleCreated>(Apply);
			Register<ToggleSwitchedOn>(Apply);
			Register<ToggleSwitchedOff>(Apply);
			Register<TagAdded>(Apply);
			Register<TagRemoved>(Apply);
		}

		//public methods which do domainy things
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

		public void SwitchOn(string user = null, string group = null)
		{
			ApplyEvent(new ToggleSwitchedOn(user, group));
		}

		public void SwitchOff(string user = null, string group = null)
		{
			ApplyEvent(new ToggleSwitchedOff(user, group));
		}

		public void AddTag(string tag)
		{
			if (_tags.Contains(tag))
				return;

			ApplyEvent(new TagAdded(tag));
		}

		public void RemoveTag(string tag)
		{
			if (_tags.Contains(tag) == false)
				return;

			ApplyEvent(new TagRemoved(tag));
		}

		protected override void PopulateExtraEventData(Event @event)
		{
			@event.UserID = _getCurrentUserID();
			base.PopulateExtraEventData(@event);
		}

		//handlers which apply the results of the domainy things
		private void Apply(ToggleCreated e)
		{
			ID = e.ID;
			Name = e.Name;
			Description = e.Description;
		}

		private void Apply(ToggleSwitchedOff e)
		{
			//_isActive = false;
			LastToggled = e.TimeStamp;

			HandleSwitching(e.User, e.Group, active: false);
		}

		private void Apply(ToggleSwitchedOn e)
		{
			LastToggled = e.TimeStamp;

			HandleSwitching(e.User, e.Group, active: true);
		}

		private void HandleSwitching(string user, string @group, bool active)
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

		private void Apply(TagAdded e) => _tags.Add(e.Name);
		private void Apply(TagRemoved e) => _tags.Remove(e.Name);
	}
}
