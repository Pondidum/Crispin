using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Storage
{
	public class Projector
	{
		private readonly Func<object> _createBlank;
		private readonly Aggregator _aggregator;
		private Dictionary<object, object> _items;

		public Projector(Type projection, Func<object> createBlank)
		{
			_createBlank = createBlank;
			_aggregator = new Aggregator(projection);
			_items = new Dictionary<object, object>();
			For = projection;
		}

		public Type For { get; }

		public void Apply(IEvent @event)
		{
			object aggregate;

			if (_items.TryGetValue(@event.AggregateID, out aggregate) == false)
				_items[@event.AggregateID] = aggregate = _createBlank();

			@event.Apply(aggregate, _aggregator);
		}

		public Dictionary<object, object> ToMemento() => _items;
		public void FromMemento(Dictionary<object, object> items) => _items = items;
	}
}
