using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOffForAnonymous : Event
	{
		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off for Anonymous Users";
		}
	}
}
