using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;
using Crispin.Projections;

namespace Crispin.Handlers.GetAll
{
	public class GetAllTogglesResponse : IStatisticGenerator
	{
		public IEnumerable<ToggleView> Toggles { get; set; }

		public GetAllTogglesResponse()
		{
			Toggles = Enumerable.Empty<ToggleView>();
		}

		public async Task Write(IStatisticsWriter writer)
		{
			foreach (var toggle in Toggles)
				await writer.WriteCount($"toggle.{toggle.ID}.queries");
		}
	}
}
