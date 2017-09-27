using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOnForGroup : Event
	{
		public EditorID Editor { get; }
		public GroupID Group { get; }

		public ToggleSwitchedOnForGroup(EditorID editor, GroupID user)
		{
			Editor = editor;
			Group = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned On for Group: {Group}";
		}
	}
}
