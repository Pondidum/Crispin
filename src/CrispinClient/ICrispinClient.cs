using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrispinClient
{
	public interface ICrispinClient
	{
		Task<IEnumerable<Toggle>> GetAllToggles();
		void SendStatistics(IEnumerable<Statistic> statistics);
	}
}
