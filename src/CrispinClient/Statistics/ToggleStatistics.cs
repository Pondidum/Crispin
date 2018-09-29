namespace CrispinClient.Statistics
{
	public class ToggleStatistics : IToggleStatistics
	{
		private readonly IStatisticsWriter _writer;

		public ToggleStatistics(IStatisticsWriter writer)
		{
			_writer = writer;
		}

		public IToggleReporter CreateReporter()
		{
			return new Reporter();
		}

		public void Complete(IToggleReporter reporter)
		{
			reporter.Complete(_writer);
		}
	}
}
