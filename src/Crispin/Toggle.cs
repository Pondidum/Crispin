using System;
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


		public Guid ID { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		private Toggle()
		{
			Register<ToggleCreated>(Apply);
		}

		//public methods which do domainy things


		//handlers which apply the results of the domainy things
		private void Apply(ToggleCreated e)
		{
			ID = e.ID;
			Name = e.Name;
			Description = e.Description;
		}
	}
}