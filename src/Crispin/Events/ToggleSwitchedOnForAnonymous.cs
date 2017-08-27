using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOnForAnonymous : Event
	{
		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned On for Anonymous Users";
		}
	}
}
