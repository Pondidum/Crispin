using System.Collections.Generic;
using Crispin.Conditions;

namespace Crispin.Handlers.RemoveCondition
{
	public class RemoveToggleConditionResponse
	{
		public ToggleID ToggleID { get; set; }
		public ConditionModes ConditionMode { get; set; }
		public IEnumerable<Condition> Conditions { get; set; }
	}
}
