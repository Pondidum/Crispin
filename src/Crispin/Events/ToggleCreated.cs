using System;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleCreated : Event
	{
		public ToggleID ID { get; }
		public string Name { get; }
		public string Description { get; }

		public ToggleCreated(ToggleID id, string name, string description)
		{
			ID = id;
			Name = name;
			Description = description;
		}

		public override string ToString()
		{
			return $"Creating Toggle '{ID}' called '{Name}' with description '{Description}'";
		}
	}
}
