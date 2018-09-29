using CrispinClient.Conditions;

namespace CrispinClient.Statistics
{
	public interface IToggleReporter
	{
		void Report(Condition condition, bool isActive);
		void Report(Toggle toggle, bool isActive);

		void Complete(IStatisticsWriter writer);
	}
}
