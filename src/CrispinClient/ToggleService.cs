using System;
using CrispinClient.Contexts;

namespace CrispinClient
{
	public class ToggleService : IToggleQuery
	{
		private readonly IToggleFetcher _fetcher;
		private readonly IToggleStatistics _statistics;

		public ToggleService(IToggleFetcher fetcher, IToggleStatistics statistics)
		{
			_fetcher = fetcher;
			_statistics = statistics;
		}

		public bool IsActive(Guid toggleID, object context) => IsActive(toggleID, new ObjectContext(context));

		public bool IsActive(Guid toggleID, IToggleContext context)
		{
			var toggles = _fetcher.GetAllToggles();

			if (toggles.TryGetValue(toggleID, out var toggle) == false)
				throw new ToggleNotFoundException(toggleID);

			var reporter = _statistics.CreateReporter(toggleID);
			var isActive = toggle.IsActive(reporter, context);

			return isActive;
		}
	}
}
