using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOn : Event
	{
		public UserID User { get; }
		public GroupID Group { get; }

		public ToggleSwitchedOn(UserID user, GroupID group)
		{
			User = user;
			Group = group;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned On for Group: {Group}, User: {User}";
		}
	}
}
