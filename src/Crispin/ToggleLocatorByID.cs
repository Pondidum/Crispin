using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;

namespace Crispin
{
	public class ToggleLocatorByID : ToggleLocator
	{
		private readonly ToggleID _toggleID;

		public ToggleLocatorByID(ToggleID toggleID)
		{
			_toggleID = toggleID;
		}

		internal override async Task<ToggleView> LocateView(IStorageSession session)
		{
			var projection = await session.LoadProjection<AllToggles>();

			return projection
				.Toggles
				.SingleOrDefault(view => view.ID == _toggleID);
		}

		internal override async Task<Toggle> LocateAggregate(IStorageSession session)
		{
			return await session.LoadAggregate<Toggle>(_toggleID);
		}
	}
}
