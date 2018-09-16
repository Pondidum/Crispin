using System;

namespace CrispinClient
{
	public interface IToggleQuery
	{
		bool IsActive(Guid toggleID, object context);
		bool IsActive(Guid toggleID, IToggleContext context);
	}
}
