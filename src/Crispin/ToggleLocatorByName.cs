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

		internal override Toggle Locate(IStorageSession session)
		{
			var allToggles = session.LoadProjection<AllToggles>().Toggles;
			var view = allToggles.SingleOrDefault(t => t.Name.Equals(_toggleName));

			return view != null
				? session.LoadAggregate<Toggle>(view.ID)
				: null;
		}
	}
}
