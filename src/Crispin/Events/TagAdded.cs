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
	}

	public class TagRemoved : Event
	{
		public string Name { get; }

		public TagRemoved(string name)
		{
			Name = name;
		}
	}
}
