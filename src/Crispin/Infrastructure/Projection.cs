﻿using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public interface IProjection
	{
		void Consume(Event @event);

		object ToMemento();
		void FromMemento(object memento);
	}

	public abstract class Projection<TMemento> : IProjection
	{
		private readonly Dictionary<Type, Action<object>> _handlers;
		private readonly List<Action<Event>> _catchAll;

		protected Projection()
		{
			_handlers = new Dictionary<Type, Action<object>>();
			_catchAll = new List<Action<Event>>();
		}

		protected void Register<TEvent>(Action<TEvent> handler) => _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
		protected void RegisterAll(Action<Event> handler) => _catchAll.Add(handler);

		public void Consume(Event @event)
		{
			Action<object> apply;
			if (_handlers.TryGetValue(@event.GetType(), out apply))
				apply(@event);

			_catchAll.ForEach(handle => handle(@event));
		}

		public object ToMemento() => CreateMemento();
		public void FromMemento(object memento) => ApplyMemento((TMemento)memento);

		protected abstract TMemento CreateMemento();
		protected abstract void ApplyMemento(TMemento memento);
	}
}
