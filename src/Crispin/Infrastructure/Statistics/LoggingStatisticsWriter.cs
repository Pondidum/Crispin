using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Crispin.Infrastructure.Statistics
{
	public class LoggingStatisticsWriter : IStatisticsWriter
	{
		private readonly ILogger<LoggingStatisticsWriter> _logger;

		public LoggingStatisticsWriter(ILogger<LoggingStatisticsWriter> logger)
		{
			_logger = logger;
		}

		public Task WriteCount(string format, params object[] parameters)
		{
			_logger.LogDebug("Statistic: " + format, parameters);
			return Task.CompletedTask;
		}
	}
}
