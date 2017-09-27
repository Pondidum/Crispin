using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOffForUser : Event
	{
		public EditorID Editor { get; }
		public UserID User { get; }

		public ToggleSwitchedOffForUser(EditorID editor, UserID user)
		{
			Editor = editor;
			User = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off for User: {User}";
		}
	}
}
