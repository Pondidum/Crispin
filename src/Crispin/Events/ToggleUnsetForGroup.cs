using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleUnsetForGroup : Event
	{
		public EditorID Editor { get; }
		public GroupID Group { get; }

		public ToggleUnsetForGroup(EditorID editor, GroupID groupID)
		{
			Editor = editor;
			Group = groupID;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID} unset for Group: {Group}";
		}
	}
}
