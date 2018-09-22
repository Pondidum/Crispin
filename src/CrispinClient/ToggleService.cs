using System;
using CrispinClient.Contexts;

namespace CrispinClient
{
	public class ToggleService : IToggleQuery
	{
		private readonly IToggleFetcher _fetcher;

		public ToggleService(IToggleFetcher fetcher)
		{
			_fetcher = fetcher;
		}

		public bool IsActive(Guid toggleID, object context) => IsActive(toggleID, new ObjectContext(context));

		public bool IsActive(Guid toggleID, IToggleContext context)
		{
			var toggles = _fetcher.GetAllToggles();

			if (toggles.TryGetValue(toggleID, out var toggle) == false)
				throw new ToggleNotFoundException(toggleID);

			return toggle.IsActive(context);
		}
	}
}
