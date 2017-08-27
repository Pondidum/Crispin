using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOnForUser : Event
	{
		public UserID User { get; }

		public ToggleSwitchedOnForUser(UserID user)
		{
			User = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned On for User: {User}";
		}
	}
}
