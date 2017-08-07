using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public abstract class AggregateRoot
	{
		private readonly Dictionary<Type, Action<object>> _handlers;
		private readonly List<object> _pendingEvents;

		protected AggregateRoot()
		{
			_handlers = new Dictionary<Type, Action<object>>();
			_pendingEvents = new List<object>();
		}

		protected void Register<TEvent>(Action<TEvent> handler) => _handlers.Add(typeof(TEvent), e => handler((TEvent)e));

		protected void ApplyEvent<TEvent>(TEvent @event)
		{
			_pendingEvents.Add(@event);
			_handlers[@event.GetType()](@event);
		}

		public IEnumerable<object> GetPendingEvents()
		{
			return _pendingEvents;
		}

		public void ClearPendingEvents()
		{
			_pendingEvents.Clear();
		}
	}
}
