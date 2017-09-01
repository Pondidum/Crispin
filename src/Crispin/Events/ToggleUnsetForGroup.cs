using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleUnsetForGroup : Event
	{
		public GroupID Group { get; }

		public ToggleUnsetForGroup(GroupID groupID)
		{
			Group = groupID;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID} unset for Group: {Group}";
		}
	}
}
