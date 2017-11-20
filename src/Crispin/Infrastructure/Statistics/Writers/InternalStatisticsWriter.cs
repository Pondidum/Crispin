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

		public Task WriteCount(IStat stat)
		{
			_store.Append(_getTimestamp(), stat);
			return Task.CompletedTask;
		}
	}
}
