using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrispinClient.Fetching
{
	public class OneTimeFetcher : IToggleFetcher
	{
		private readonly Lazy<Task<IReadOnlyDictionary<Guid, Toggle>>> _toggles;

		public OneTimeFetcher(ICrispinClient client)
		{
			_toggles = new Lazy<Task<IReadOnlyDictionary<Guid, Toggle>>>(async () =>
			{
				var toggles = await client.GetAllToggles();
				return toggles.ToDictionary(t => t.ID);
			});
		}

		public Task<IReadOnlyDictionary<Guid, Toggle>> GetAllToggles() => _toggles.Value;
	}
}
