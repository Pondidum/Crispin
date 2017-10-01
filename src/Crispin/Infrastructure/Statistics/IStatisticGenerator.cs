using System.Threading.Tasks;

namespace Crispin.Infrastructure.Statistics
{
	public interface IStatisticGenerator<TRequest, TResponse>
	{
		Task Write(IStatisticsWriter writer, TRequest request, TResponse response);
	}
}
