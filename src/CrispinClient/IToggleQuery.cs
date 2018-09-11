using System;

namespace CrispinClient
{
	public interface IToggleQuery
	{
		bool IsActive(Guid toggleID, object query);
	}
}
