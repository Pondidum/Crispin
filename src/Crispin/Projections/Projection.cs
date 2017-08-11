using System;
using System.Collections.Generic;

namespace Crispin.Projections
{
	public abstract class Projection
	{
		private readonly Dictionary<Type, Action<object>> _handlers;

		protected Projection()
		{
			_handlers = new Dictionary<Type, Action<object>>();
		}

		protected void Register<TEvent>(Action<TEvent> handler) => _handlers.Add(typeof(TEvent), e => handler((TEvent)e));

		public void Consume(object @event)
		{
			Action<object> apply;
			if (_handlers.TryGetValue(@event.GetType(), out apply))
				apply(@event);
		}
	}
}
