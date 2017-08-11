namespace Crispin.Events
{
	public class TagAdded
	{
		public string Name { get; }

		public TagAdded(string name)
		{
			Name = name;
		}
	}

	public class TagRemoved
	{
		public string Name { get; }

		public TagRemoved(string name)
		{
			Name = name;
		}
	}
}
