using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Views;

namespace Crispin
{
	public abstract class ToggleLocator
	{
		public static ToggleLocator Create(ToggleID id) => new ToggleLocatorByID(id);
		public static ToggleLocator Create(string name) => new ToggleLocatorByName(name);

		internal abstract Task<ToggleView> LocateView(IStorageSession session);
		internal abstract Task<Toggle> LocateAggregate(IStorageSession session);
	}
}
