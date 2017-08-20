using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOn : Event
	{
		public string User { get; }
		public string Group { get; }

		public ToggleSwitchedOn(string user = null, string group = null)
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
