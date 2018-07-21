using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public abstract class AggregateRoot : IEvented
	{
		private readonly Dictionary<Type, Action<Event>> _handlers;
		private readonly List<Event> _pendingEvents;

		protected AggregateRoot()
		{
			_handlers = new Dictionary<Type, Action<Event>>();
			_pendingEvents = new List<Event>();
		}

		public ToggleID ID { get; protected set; }

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

			PopulateExtraEventData(@event);
		}

		protected virtual void PopulateExtraEventData(Event @event)
		{
			@event.AggregateID = ID;
		}

		IEnumerable<Event> IEvented.GetPendingEvents()
		{
			return _pendingEvents;
		}

		void IEvented.ClearPendingEvents()
		{
			_pendingEvents.Clear();
		}

		void IEvented.LoadFromEvents(IEnumerable<Event> events)
		{
			foreach (var @event in events)
				_handlers[@event.GetType()](@event);
		}
	}
}
