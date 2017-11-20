using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public class InMemoryStatisticsStore : IStatisticsStore
	{
		private readonly List<StatWrapper> _stats;

		public InMemoryStatisticsStore()
		{
			_stats = new List<StatWrapper>();
		}

		public Task Append(DateTime timestamp, IStat stat)
		{
			_stats.Add(new StatWrapper(timestamp, stat));
			return Task.CompletedTask;
		}

		private struct StatWrapper
		{
			public DateTime Timestamp { get; }
			public IStat Stat { get; }

			public StatWrapper(DateTime timestamp, IStat stat)
			{
				Timestamp = timestamp;
				Stat = stat;
			}
		}
	}
}
