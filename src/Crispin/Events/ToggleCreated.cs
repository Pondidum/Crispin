using System;

namespace Crispin.Events
{
	public class ToggleCreated
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
	}
}
