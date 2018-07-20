using System;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleCreated : Event
	{
		public EditorID Creator { get; }
		public string Name { get; }
		public string Description { get; }

		public ToggleCreated(EditorID creator, ToggleID id, string name, string description)
		{
			AggregateID = id;
			Creator = creator;
			Name = name;
			Description = description;
		}

		public override string ToString()
		{
			return $"Creating Toggle called '{Name}' with description '{Description}'";
		}
	}
}
