using System.Collections.Generic;

namespace CrispinClient.Fetching
{
	public interface ICrispinClient
	{
		IEnumerable<Toggle> GetAllToggles();
	}
}
