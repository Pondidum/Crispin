using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrispinClient
{
	public interface ICrispinClient
	{
		Task<IEnumerable<Toggle>> GetAllToggles();
		Task SendStatistics(IEnumerable<Statistic> statistics);
	}
}
