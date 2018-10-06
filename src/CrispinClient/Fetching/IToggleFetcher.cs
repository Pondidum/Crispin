using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrispinClient.Fetching
{
	public interface IToggleFetcher
	{
		Task<IReadOnlyDictionary<Guid, Toggle>> GetAllToggles();
	}
}
