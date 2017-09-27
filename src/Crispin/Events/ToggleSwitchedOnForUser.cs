using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOnForUser : Event
	{
		public EditorID Editor { get; }
		public UserID User { get; }

		public ToggleSwitchedOnForUser(EditorID editor, UserID user)
		{
			Editor = editor;
			User = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned On for User: {User}";
		}
	}
}
