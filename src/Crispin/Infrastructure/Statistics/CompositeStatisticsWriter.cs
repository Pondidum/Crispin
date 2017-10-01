using System.Linq;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public class CompositeStatisticsWriter : IStatisticsWriter
	{
		private readonly IStatisticsWriter[] _writers;

		public CompositeStatisticsWriter(params IStatisticsWriter[] writers)
		{
			_writers = writers.ToArray();
		}

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
