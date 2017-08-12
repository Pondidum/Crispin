using System;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleCreated : Event
	{
		public Guid ID { get; }
		public string Name { get; }
		public string Description { get; }

		public ToggleCreated(Guid id, string name, string description)
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
