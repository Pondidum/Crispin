using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics.Writers
{
	public class CompositeStatisticsWriter : IStatisticsWriter
	{
		private readonly IStatisticsWriter[] _writers;

		public CompositeStatisticsWriter(IEnumerable<IStatisticsWriter> writers)
		{
			_writers = writers.ToArray();
		}

		public IEnumerable<IStatisticsWriter> Writers => _writers;

		public async Task WriteCount(IStat stat)
		{
			await Task.WhenAll(_writers
				.Select(writer => writer.WriteCount(stat))
				.ToArray());
		}
	}
}
