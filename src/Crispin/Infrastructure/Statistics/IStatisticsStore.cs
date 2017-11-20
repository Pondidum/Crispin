using System;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public interface IStatisticsStore
	{
		Task Append(DateTime timestamp, IStat stat);
	}
}
