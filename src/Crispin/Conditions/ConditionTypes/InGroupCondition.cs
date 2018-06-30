using System.Collections.Generic;

namespace Crispin.Conditions.ConditionTypes
{
	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }

		public override IEnumerable<string> Validate()
		{
			if (string.IsNullOrWhiteSpace(SearchKey))
				yield return $"'{nameof(SearchKey)}' is missing";

			if (string.IsNullOrWhiteSpace(GroupName))
				yield return $"'{nameof(GroupName)}' is missing";
		}
	}
}
