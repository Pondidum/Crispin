using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public interface IEvented
	{
		IEnumerable<IEvent> GetPendingEvents();
		void ClearPendingEvents();
	}
}
