using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOnForGroup : Event
	{
		public GroupID Group { get; }

		public ToggleSwitchedOnForGroup(GroupID user)
		{
			Group = user;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned On for Group: {Group}";
		}
	}
}
