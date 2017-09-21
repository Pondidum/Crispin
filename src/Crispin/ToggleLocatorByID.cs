using Crispin.Infrastructure.Storage;

namespace Crispin
{
	public class ToggleLocatorByID : ToggleLocator
	{
		private readonly ToggleID _toggleID;

		public ToggleLocatorByID(ToggleID toggleID)
		{
			_toggleID = toggleID;
		}

		internal override Toggle Locate(IStorageSession session)
		{
			return session.LoadAggregate<Toggle>(_toggleID);
		}
	}
}
