using System;
using System.Linq;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;

namespace Crispin
{
	public class ToggleLocatorByName : ToggleLocator
	{
		private readonly string _toggleName;

		public ToggleLocatorByName(string toggleName)
		{
			_toggleName = toggleName;
		}

		internal override ToggleView LocateView(IStorageSession session)
		{
			return session
				.LoadProjection<AllToggles>()
				.Toggles
				.SingleOrDefault(t => t.Name.Equals(_toggleName, StringComparison.OrdinalIgnoreCase));
		}

		internal override Toggle LocateAggregate(IStorageSession session)
		{
			var view = LocateView(session);

			return view != null
				? session.LoadAggregate<Toggle>(view.ID)
				: null;
		}
	}
}
