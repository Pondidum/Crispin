using CrispinClient.Conditions;

namespace CrispinClient
{
	public interface IToggleReporter
	{
		void Report(Condition condition, bool isActive);
	}
}
