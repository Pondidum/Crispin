using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;
using Crispin.Projections;

namespace Crispin.Handlers.GetSingle
{
	public class GetToggleResponse : IStatisticGenerator
	{
		public ToggleView Toggle { get; set; }

		public async Task Write(IStatisticsWriter writer)
		{
			if (Toggle == null)
				return;

			await writer.WriteCount($"toggle.{Toggle.ID}.queries");
		}
	}
}
