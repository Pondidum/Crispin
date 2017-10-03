using System;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public interface IStatisticsWriter
	{
		Task WriteCount(string format, params object[] parameters);
	}
}
