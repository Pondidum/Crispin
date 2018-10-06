using System.Threading.Tasks;

namespace CrispinClient.Statistics
{
	public interface IStatisticsWriter
	{
		Task Write(Statistic statistic);
	}
}
