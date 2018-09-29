using System;
using CrispinClient.Conditions;

namespace CrispinClient.Statistics
{
	public class Reporter : IToggleReporter
	{
		private readonly Statistic _statistic;

		public Reporter()
		{
			_statistic = new Statistic();
		}

		public void Report(Condition condition, bool isActive)
		{
			_statistic.ConditionStates[condition.ID] = isActive;
		}

		public void Report(Toggle toggle, bool isActive)
		{
			_statistic.ToggleID = toggle.ID;
			_statistic.Timestamp = DateTime.Now;
			_statistic.Active = isActive;
		}

		public void Complete(IStatisticsWriter writer)
		{
			writer.Write(_statistic);
		}
	}
}
