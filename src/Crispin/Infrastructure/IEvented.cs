using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public interface IEvented
	{
		IEnumerable<IAct> GetPendingEvents();
		void ClearPendingEvents();
	}
}
