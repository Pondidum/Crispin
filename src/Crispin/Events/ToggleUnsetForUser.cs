using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleUnsetForUser : Event
	{
		public EditorID Editor { get; }
		public UserID User { get; }

		public ToggleUnsetForUser(EditorID editor, UserID userID)
		{
			Editor = editor;
			User = userID;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID} unset for User: {User}";
		}
	}
}
