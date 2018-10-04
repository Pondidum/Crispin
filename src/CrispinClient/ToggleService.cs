using System;
using CrispinClient.Contexts;
using CrispinClient.Fetching;
using CrispinClient.Statistics;

namespace CrispinClient
{
	public class ToggleService
	{
		private readonly IToggleFetcher _fetcher;
		private readonly IStatisticsWriter _writer;

		public ToggleService(IToggleFetcher fetcher, IStatisticsWriter writer)
		{
			_fetcher = fetcher;
			_writer = writer;
		}

		public bool IsActive(Guid toggleID, object context) => IsActive(toggleID, new ObjectContext(context));

		public bool IsActive(Guid toggleID, IToggleContext context)
		{
			var toggles = _fetcher.GetAllToggles();

			if (toggles.TryGetValue(toggleID, out var toggle) == false)
				throw new ToggleNotFoundException(toggleID);

			return toggle.IsActive(_writer, context);
		}
	}
}
