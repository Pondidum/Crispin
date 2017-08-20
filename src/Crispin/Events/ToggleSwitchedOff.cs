using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOff : Event
	{
		public string User { get; }
		public string Group { get; }

		public ToggleSwitchedOff(string user = null, string group = null)
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