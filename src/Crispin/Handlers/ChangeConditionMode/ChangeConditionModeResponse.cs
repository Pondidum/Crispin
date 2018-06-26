using System.Collections.Generic;
using Crispin.Conditions;

namespace Crispin.Handlers.ChangeConditionMode
{
	public class ChangeConditionModeResponse
	{
		public ConditionModes ConditionMode { get; set; }
		public IEnumerable<Condition> Conditions { get; set; }
	}
}
