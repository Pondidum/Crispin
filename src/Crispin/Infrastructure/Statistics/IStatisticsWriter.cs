using System;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public interface IStatisticsWriter
	{
		Task WriteCount(IStat stat);
	}

	public interface IStat
	{
		string ToString();
	}
}
