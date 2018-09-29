using System;
using CrispinClient.Contexts;
using CrispinClient.Fetching;
using CrispinClient.Statistics;

namespace CrispinClient
{
	public class ToggleService
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

			var reporter = _statistics.CreateReporter();
			var isActive = toggle.IsActive(reporter, context);

			_statistics.Complete(reporter);

			return isActive;
		}
	}
}
