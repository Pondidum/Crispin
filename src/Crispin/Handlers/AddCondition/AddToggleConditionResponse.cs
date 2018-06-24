using System.Collections.Generic;
using Crispin.Conditions;

namespace Crispin.Handlers.AddCondition
{
	public class AddToggleConditionResponse
	{
		public ConditionModes ConditionMode { get; set; }
		public IEnumerable<Condition> Conditions { get; set; }
	}
}
