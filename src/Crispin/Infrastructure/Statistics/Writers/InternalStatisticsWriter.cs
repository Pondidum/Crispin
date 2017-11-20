using System;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics.Writers
{
	public class InternalStatisticsWriter : IStatisticsWriter
	{
		private readonly IStatisticsStore _store;
		private readonly Func<DateTime> _getTimestamp;

		public InternalStatisticsWriter(IStatisticsStore store, Func<DateTime> getTimestamp)
		{
			_store = store;
			_getTimestamp = getTimestamp;
		}

		public async Task WriteCount(IStat stat)
		{
			await _store.Append(_getTimestamp(), stat);
		}
	}
}
