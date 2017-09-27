using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class TagRemoved : Event
	{
		public EditorID Editor { get; }
		public string Name { get; }

		public TagRemoved(EditorID editor, string name)
		{
			Editor = editor;
			Name = name;
		}

		public override string ToString()
		{
			return $"Removed Tag '{Name}' from Toggle '{AggregateID}'";
		}
	}
}