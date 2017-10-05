using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;

namespace Crispin.Handlers.GetSingle
{
	public class GetToggleStatistics : IStatisticGenerator<GetToggleRequest, GetToggleResponse>
	{
		public async Task Write(IStatisticsWriter writer, GetToggleRequest request, GetToggleResponse response)
		{
			if (response.Toggle == null)
				return;

			await writer.WriteCount("toggle.{toggleID}.read", response.Toggle.ID);
		}
	}
}
