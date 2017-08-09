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
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Toggles must have a non-whitespace name.");

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


		public Guid ID { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public bool Active { get; private set; }

		private Toggle()
		{
			Register<ToggleCreated>(Apply);
			Register<ToggleSwitchedOn>(Apply);
			Register<ToggleSwitchedOff>(Apply);
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
		}

		private void Apply(ToggleSwitchedOn e)
		{
			Active = true;
		}
	}
}
