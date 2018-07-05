using System.Collections.Generic;

namespace Crispin.Conditions
{
	public interface IConditionBuilder
	{
		IEnumerable<string> CanCreateFrom(Dictionary<string, object> conditionProperties);
		Condition CreateCondition(ConditionID conditionID, Dictionary<string, object> conditionProperties);
	}
}
