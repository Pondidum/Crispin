using System;
using System.Collections.Generic;
using System.Linq;

namespace Crispin.Infrastructure
{
	public class EventAdaptor<T> : IEvented
	{
		private readonly IEnumerable<T> _collection;
		private readonly Func<T, IEvent> _createEvent;
		private bool _cleared;

		public EventAdaptor(IEnumerable<T> collection, Func<T, IEvent> createEvent)
		{
			_collection = collection;
			_createEvent = createEvent;
			_cleared = false;
		}

		public IEnumerable<IEvent> GetPendingEvents() => _cleared
			? Enumerable.Empty<IEvent>()
			: _collection.Select(_createEvent);

		public void ClearPendingEvents() => _cleared = true;
	}
}
