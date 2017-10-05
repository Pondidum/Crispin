using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;

namespace Crispin.Handlers.GetAll
{
	public class GetAllTogglesStatistics : IStatisticGenerator<GetAllTogglesRequest, GetAllTogglesResponse>
	{
		public async Task Write(IStatisticsWriter writer, GetAllTogglesRequest request, GetAllTogglesResponse response)
		{
			foreach (var toggle in response.Toggles)
				await writer.WriteCount("toggle.{toggleID}.read", toggle.ID);

		}
	}
}
