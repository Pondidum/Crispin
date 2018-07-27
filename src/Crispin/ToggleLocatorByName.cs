using System;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Views;

namespace Crispin
{
	public class ToggleLocatorByName : ToggleLocator
	{
		private readonly string _toggleName;

		public ToggleLocatorByName(string toggleName)
		{
			_toggleName = toggleName;
		}

		internal override async Task<ToggleView> LocateView(IStorageSession session)
		{
			var projection = await session.QueryProjection<ToggleView>();

			return projection
				.SingleOrDefault(t => t.Name.Equals(_toggleName, StringComparison.OrdinalIgnoreCase));
		}

		internal override async Task<Toggle> LocateAggregate(IStorageSession session)
		{
			var view = await LocateView(session);

			return view != null
				? await session.LoadAggregate<Toggle>(view.ID)
				: null;
		}
	}
}
