using System;
using System.Linq;
using CrispinClient.Conditions;
using CrispinClient.Statistics;

namespace CrispinClient
{
	public class Toggle
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public Condition[] Conditions { get; set; }
		public ConditionModes ConditionMode { get; set; }

		public bool IsActive(IStatisticsWriter writer, IToggleContext context)
		{
			var stats = new Statistic
			{
				ToggleID = ID,
				Timestamp = DateTime.Now,
				User = context.GetCurrentUser()
			};

			var isActive = ConditionMode == ConditionModes.Any
				? Conditions.Any(c => c.IsMatch(stats, context))
				: Conditions.All(c => c.IsMatch(stats, context));

			stats.Active = isActive;

			writer.Write(stats);

			return isActive;
		}
	}
}
