using System;
using System.Collections.Generic;
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
		private readonly HashSet<string> _usersActive;
		private readonly HashSet<string> _usersInActive;

		private Toggle(Func<string> getCurrentUserID)
		{
			_getCurrentUserID = getCurrentUserID;
			_tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			_usersActive = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_usersInActive = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			Register<ToggleCreated>(Apply);
			Register<ToggleSwitchedOn>(Apply);
			Register<ToggleSwitchedOff>(Apply);
			Register<TagAdded>(Apply);
			Register<TagRemoved>(Apply);
		}

		//public methods which do domainy things
		public bool IsActive(string userID)
		{
			if (_usersInActive.Contains(userID))
				return false;

			if (_usersActive.Contains(userID))
				return true;

			return false;
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

			if (string.IsNullOrWhiteSpace(e.User) == false)
			{
				_usersActive.Remove(e.User);
				_usersInActive.Add(e.User);
			}
		}

		private void Apply(ToggleSwitchedOn e)
		{
			LastToggled = e.TimeStamp;

			if (string.IsNullOrWhiteSpace(e.User) == false)
			{
				_usersActive.Add(e.User);
				_usersInActive.Remove(e.User);
			}
		}

		private void Apply(TagAdded e) => _tags.Add(e.Name);
		private void Apply(TagRemoved e) => _tags.Remove(e.Name);
	}
}
