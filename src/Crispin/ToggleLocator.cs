using Crispin.Infrastructure.Storage;

namespace Crispin
{
	public abstract class ToggleLocator
	{
		public static ToggleLocator Create(ToggleID id) => new ToggleLocatorByID(id);
		public static ToggleLocator Create(string name) => new ToggleLocatorByName(name);

		internal abstract Toggle Locate(IStorageSession session);
	}
}
