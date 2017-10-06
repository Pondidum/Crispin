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

			await writer.WriteCount(new ToggleRead(response.Toggle.ID));
		}
	}

	public struct ToggleRead : IStat
	{
		public ToggleID ToggleID { get; }

		public ToggleRead(ToggleID toggleID)
		{
			ToggleID = toggleID;
		}

		public override string ToString()
		{
			return $"Toggle '{ToggleID} was read";
		}
	}
}
