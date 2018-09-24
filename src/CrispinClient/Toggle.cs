using System;
using CrispinClient.Conditions;

namespace CrispinClient
{
	public class Toggle
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public Condition[] Conditions { get; set; }
		public ConditionModes ConditionMode { get; set; }

		public bool IsActive(IToggleReporter reporter, IToggleContext context)
		{
			var any = ConditionMode == ConditionModes.Any
				? new AnyCondition() as Condition
				: new AllCondition() as Condition;

			any.Children = Conditions;

			return any.IsMatch(reporter, context);
		}
	}
}
