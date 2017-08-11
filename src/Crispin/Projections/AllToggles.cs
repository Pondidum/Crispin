using System;
using System.Collections.Generic;
using Crispin.Events;

namespace Crispin.Projections
{
	public class AllToggles
	{
		public IEnumerable<KeyValuePair<Guid, string>> Toggles => _toggles;

		private readonly Dictionary<Guid, string> _toggles;
		private readonly Dictionary<Type, Action<object>> _handlers;

		public AllToggles()
		{
			_toggles = new Dictionary<Guid, string>();
			_handlers = new Dictionary<Type, Action<object>>();

			Register<ToggleCreated>(Apply);
		}

		protected void Register<TEvent>(Action<TEvent> handler) => _handlers.Add(typeof(TEvent), e => handler((TEvent)e));

		public void Consume(object @event)
		{
			Action<object> apply;
			if (_handlers.TryGetValue(@event.GetType(), out apply))
				apply(@event);
		}

		private void Apply(ToggleCreated e) => _toggles[e.ID] = e.Name;
	}
}
