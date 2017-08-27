using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOffForUser : Event
	{
		public UserID User { get; }

		public ToggleSwitchedOffForUser(UserID user)
		{
			User = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off for User: {User}";
		}
	}
}
