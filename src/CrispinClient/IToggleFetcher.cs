using System;
using System.Collections.Generic;

namespace CrispinClient
{
	public interface IToggleFetcher
	{
		IReadOnlyDictionary<Guid, Toggle> GetAllToggles();
	}
}
