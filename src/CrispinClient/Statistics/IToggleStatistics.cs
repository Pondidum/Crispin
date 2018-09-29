namespace CrispinClient.Statistics
{
	public interface IToggleStatistics
	{
		IToggleReporter CreateReporter();
		void Complete(IToggleReporter reporter);
	}
}
