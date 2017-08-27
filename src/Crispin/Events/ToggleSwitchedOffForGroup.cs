using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOffForGroup : Event
	{
		public GroupID Group { get; }

		public ToggleSwitchedOffForGroup(GroupID user)
		{
			Group = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off for Group: {Group}";
		}
	}
}
