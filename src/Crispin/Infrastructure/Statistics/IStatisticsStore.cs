using System;

namespace Crispin.Infrastructure.Statistics
{
	public interface IStatisticsStore
	{
		void Append(DateTime timestamp, IStat stat);
	}
}
