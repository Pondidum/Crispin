using System;
using System.Linq;
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
			return ConditionMode == ConditionModes.Any
				? Conditions.Any(c => c.IsMatch(reporter, context))
				: Conditions.All(c => c.IsMatch(reporter, context));
		}
	}
}
