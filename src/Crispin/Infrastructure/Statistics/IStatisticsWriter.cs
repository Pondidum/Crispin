using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public interface IStatisticsWriter
	{
		Task Write(string key, object value);
	}
}
