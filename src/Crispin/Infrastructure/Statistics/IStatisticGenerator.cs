using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public interface IStatisticGenerator
	{
		Task Write(IStatisticsWriter writer);
	}
}
