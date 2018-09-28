using System;
using System.Collections.Generic;

namespace CrispinClient.Fetching
{
	public interface IToggleFetcher
	{
		IReadOnlyDictionary<Guid, Toggle> GetAllToggles();
	}
}
