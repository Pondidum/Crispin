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

		public Task Write(string key, string value)
		{
			_logger.LogDebug("Statistic: {key}: {value}", key, value);
			return Task.CompletedTask;
		}

		public Task WriteCount(string key)
		{
			_logger.LogDebug("Statistic: {key}++", key);
			return Task.CompletedTask;
		}
	}
}
