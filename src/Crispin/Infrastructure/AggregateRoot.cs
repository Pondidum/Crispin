using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public abstract class AggregateRoot : IEvented
	{
		private readonly Dictionary<Type, Action<object>> _handlers;
		private readonly List<object> _pendingEvents;

		protected AggregateRoot()
		{
			_handlers = new Dictionary<Type, Action<object>>();
			_pendingEvents = new List<object>();
		}

		public Guid ID { get; protected set; }

		protected void Register<TEvent>(Action<TEvent> handler) 
			where TEvent : Event
		{
			_handlers.Add(typeof(TEvent), e => handler((TEvent)e));
		}

		protected void ApplyEvent<TEvent>(TEvent @event)
			where TEvent : Event
		{
			@event.TimeStamp = DateTime.Now;

			_pendingEvents.Add(@event);
			_handlers[@event.GetType()](@event);

			@event.AggregateID = ID;
		}

		IEnumerable<object> IEvented.GetPendingEvents()
		{
			return _pendingEvents;
		}

		void IEvented.ClearPendingEvents()
		{
			_pendingEvents.Clear();
		}

		void IEvented.LoadFromEvents(IEnumerable<object> events)
		{
			foreach (var @event in events)
				_handlers[@event.GetType()](@event);
		}
	}
}
