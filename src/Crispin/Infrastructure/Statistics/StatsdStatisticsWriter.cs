using System.Threading.Tasks;
using StatsdClient;

namespace Crispin.Infrastructure.Statistics
{
	public class StatsdStatisticsWriter : IStatisticsWriter
	{
		public StatsdStatisticsWriter()
		{
			Metrics.Configure(new MetricsConfig
			{
				Prefix = "crispin",
				StatsdServerName = "localhost"
			});
		}

		public async Task Write(string key, string value)
		{
			await Task.Run(() => Metrics.Set(key, value));
		}

		public async Task WriteCount(string key)
		{
			await Task.Run(() => Metrics.Counter(key));
		}
	}
}
