using Crispin.Infrastructure.Storage;
using Crispin.Projections;

namespace Crispin
{
	public abstract class ToggleLocator
	{
		public static ToggleLocator Create(ToggleID id) => new ToggleLocatorByID(id);
		public static ToggleLocator Create(string name) => new ToggleLocatorByName(name);

		internal abstract ToggleView LocateView(IStorageSession session);
		internal abstract Toggle LocateAggregate(IStorageSession session);
	}
}
