using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public class AggregateRoot
	{
		private readonly Dictionary<Type, Action<object>> _handlers;

		protected AggregateRoot()
		{
			_handlers = new Dictionary<Type, Action<object>>();
		}

		protected void Register<TEvent>(Action<TEvent> handler) => _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
		protected void ApplyEvent<TEvent>(TEvent @event) => _handlers[@event.GetType()](@event);
	}
}