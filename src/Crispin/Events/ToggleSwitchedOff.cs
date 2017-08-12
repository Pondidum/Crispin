using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOff : Event
	{
		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off";
		}
	}
}