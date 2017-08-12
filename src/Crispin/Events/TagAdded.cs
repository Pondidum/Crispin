using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class TagAdded : Event
	{
		public string Name { get; }

		public TagAdded(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return $"Added Tag '{Name}' to Toggle '{AggregateID}'";
		}
	}
}
