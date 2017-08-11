using System;
using System.Collections.Generic;
using Crispin.Events;
using Crispin.Infrastructure;

namespace Crispin
{
	public class Toggle : AggregateRoot
	{
		public static Toggle CreateNew(string name, string description = "")
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name), "Toggles must have a non-whitespace name.");

			var toggle = new Toggle();
			toggle.ApplyEvent(new ToggleCreated(Guid.NewGuid(), name.Trim(), description));

			return toggle;
		}

		public static Toggle LoadFrom(IEnumerable<object> events)
		{
			var toggle = new Toggle();
			((IEvented)toggle).LoadFromEvents(events);

			return toggle;
		}


		public string Name { get; private set; }
		public string Description { get; private set; }
		public bool Active { get; private set; }
		public DateTime? LastToggled { get; private set; }
		public IEnumerable<string> Tags => _tags;

		private readonly HashSet<string> _tags;

		private Toggle()
		{
			_tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			Register<ToggleCreated>(Apply);
			Register<ToggleSwitchedOn>(Apply);
			Register<ToggleSwitchedOff>(Apply);
			Register<TagAdded>(Apply);
			Register<TagRemoved>(Apply);
		}

		//public methods which do domainy things

		public void SwitchOn()
		{
			if (Active)
				return;

			ApplyEvent(new ToggleSwitchedOn());
		}

		public void SwitchOff()
		{
			if (Active == false)
				return;

			ApplyEvent(new ToggleSwitchedOff());
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

		//handlers which apply the results of the domainy things
		private void Apply(ToggleCreated e)
		{
			ID = e.ID;
			Name = e.Name;
			Description = e.Description;
		}

		private void Apply(ToggleSwitchedOff e)
		{
			Active = false;
			LastToggled = e.TimeStamp;
		}

		private void Apply(ToggleSwitchedOn e)
		{
			Active = true;
			LastToggled = e.TimeStamp;
		}

		private void Apply(TagAdded e) => _tags.Add(e.Name);
		private void Apply(TagRemoved e) => _tags.Remove(e.Name);
	}
}
