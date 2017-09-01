using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleUnsetForUser : Event
	{
		public UserID User { get; }

		public ToggleUnsetForUser(UserID userID)
		{
			User = userID;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID} unset for User: {User}";
		}
	}
}
