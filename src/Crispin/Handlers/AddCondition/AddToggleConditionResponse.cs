using System.Collections.Generic;
using Crispin.Conditions;

namespace Crispin.Handlers.AddCondition
{
	public class AddToggleConditionResponse
	{
		public ToggleID ToggleID { get; set; }
		public ConditionID AddedConditionID { get; set; }
		public IEnumerable<Condition> Conditions { get; set; }
	}
}
