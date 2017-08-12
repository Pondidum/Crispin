using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class TagRemoved : Event
	{
		public string Name { get; }

		public TagRemoved(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return $"Removed Tag '{Name}' from Toggle '{AggregateID}'";
		}
	}
}