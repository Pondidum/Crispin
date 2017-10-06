using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;

namespace Crispin.Handlers.GetAll
{
	public class GetAllTogglesStatistics : IStatisticGenerator<GetAllTogglesRequest, GetAllTogglesResponse>
	{
		public async Task Write(IStatisticsWriter writer, GetAllTogglesRequest request, GetAllTogglesResponse response)
		{
			await writer.WriteCount(new MultipleTogglesRead(
				response.Toggles.Select(toggle => toggle.ID)
			));
		}
	}

	public struct MultipleTogglesRead : IStat
	{
		public IEnumerable<ToggleID> Toggles { get; }

		public MultipleTogglesRead(IEnumerable<ToggleID> toggles)
		{
			Toggles = toggles;
		}

		public override string ToString()
		{
			return $"Toggles '{string.Join(", ", Toggles)}' was read";
		}
	}
}
