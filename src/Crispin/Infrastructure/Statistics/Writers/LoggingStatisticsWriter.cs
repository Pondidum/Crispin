using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Crispin.Infrastructure.Statistics.Writers
{
	public class LoggingStatisticsWriter : IStatisticsWriter
	{
		private readonly ILogger<LoggingStatisticsWriter> _logger;

		public LoggingStatisticsWriter(ILogger<LoggingStatisticsWriter> logger)
		{
			_logger = logger;
		}

		public Task WriteCount(IStat stat)
		{
			_logger.LogDebug("Statistic: " + stat.ToString());
			return Task.CompletedTask;
		}
	}
}
