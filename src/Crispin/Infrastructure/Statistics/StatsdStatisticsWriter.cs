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

		public async Task WriteCount(string format, params object[] parameters)
		{
			await Task.Run(() => Metrics.Counter(format.Render(parameters)));
		}
	}
}
