using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOff : Event
	{
		public UserID User { get; }
		public string Group { get; }

		public ToggleSwitchedOff(UserID user, string group = null)
		{
			User = user;
			Group = group;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off for Group: {Group}, User: {User}";
		}
	}
}