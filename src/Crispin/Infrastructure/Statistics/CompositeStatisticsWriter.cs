using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatsdClient;

namespace Crispin.Infrastructure.Statistics
{
	public class CompositeStatisticsWriter : IStatisticsWriter
	{
		private readonly IStatisticsWriter[] _writers;

		public CompositeStatisticsWriter(LoggingStatisticsWriter logging, StatsdStatisticsWriter statsd)
		{
			_writers = new IStatisticsWriter[]
			{
				logging, statsd
			};
		}

		public IEnumerable<IStatisticsWriter> Writers => _writers;

		public async Task Write(string key, string value)
		{
			await Task.WhenAll(_writers
				.Select(writer => writer.Write(key, value))
				.ToArray());
		}

		public async Task WriteCount(string key)
		{
			await Task.WhenAll(_writers
				.Select(writer => writer.WriteCount(key))
				.ToArray());
		}
	}
}
