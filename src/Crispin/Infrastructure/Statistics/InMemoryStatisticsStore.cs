using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Statistics
{
	public class InMemoryStatisticsStore : IStatisticsStore
	{
		private readonly List<StatWrapper> _stats;

		public InMemoryStatisticsStore()
		{
			_stats = new List<StatWrapper>();
		}

		public void Append(DateTime timestamp, IStat stat)
		{
			_stats.Add(new StatWrapper(timestamp, stat));
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
