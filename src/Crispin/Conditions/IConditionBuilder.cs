using System.Collections.Generic;

namespace Crispin.Conditions
{
	public interface IConditionBuilder
	{
		Condition CreateCondition(Dictionary<string, object> conditionProperties);
	}
}
