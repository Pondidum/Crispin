using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public abstract class Projection<TMemento> : IProjection
	{
		private readonly Dictionary<Type, Action<object>> _handlers;

		protected Projection()
		{
			_handlers = new Dictionary<Type, Action<object>>();
		}

		protected void Register<TEvent>(Action<TEvent> handler) => _handlers.Add(typeof(TEvent), e => handler((TEvent)e));

		public void Consume(Event @event)
		{
			if (_handlers.TryGetValue(@event.GetType(), out var apply))
				apply(@event);
		}

		public object ToMemento() => CreateMemento();
		public void FromMemento(object memento) => ApplyMemento((TMemento)memento);

		protected abstract TMemento CreateMemento();
		protected abstract void ApplyMemento(TMemento memento);
	}
}
