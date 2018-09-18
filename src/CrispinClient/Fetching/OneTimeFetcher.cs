using System;
using System.Collections.Generic;
using System.Linq;

namespace CrispinClient.Fetching
{
	public class OneTimeFetcher : IToggleFetcher
	{
		private readonly Lazy<Dictionary<Guid, Toggle>> _toggles;

		public OneTimeFetcher(ICrispinClient client)
		{
			_toggles = new Lazy<Dictionary<Guid, Toggle>>(() => client
				.GetAllToggles()
				.ToDictionary(t => t.ID));
		}

		public IReadOnlyDictionary<Guid, Toggle> GetAllToggles() => _toggles.Value;
	}
}
