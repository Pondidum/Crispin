using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOffForGroup : Event
	{
		public EditorID Editor { get; }
		public GroupID Group { get; }

		public ToggleSwitchedOffForGroup(EditorID editor, GroupID user)
		{
			Editor = editor;
			Group = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off for Group: {Group}";
		}
	}
}
