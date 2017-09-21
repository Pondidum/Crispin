using System.Linq;
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

		internal override ToggleView LocateView(IStorageSession session)
		{
			return session
				.LoadProjection<AllToggles>()
				.Toggles
				.SingleOrDefault(view => view.ID == _toggleID);
		}

		internal override Toggle LocateAggregate(IStorageSession session)
		{
			return session.LoadAggregate<Toggle>(_toggleID);
		}
	}
}
