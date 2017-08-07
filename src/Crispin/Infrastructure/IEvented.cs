using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public interface IEvented
	{
		IEnumerable<object> GetPendingEvents();
		void ClearPendingEvents();
		void LoadFromEvents(IEnumerable<object> events);
	}
}
