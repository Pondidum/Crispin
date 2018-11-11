using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Crispin.Infrastructure.Storage
{
	public class Projector
	{
		private readonly Func<object> _createBlank;
		private readonly Aggregator _aggregator;
		private ConcurrentDictionary<object, object> _items;

		public Projector(Type projection, Func<object> createBlank)
		{
			_createBlank = createBlank;
			_aggregator = new Aggregator(projection);
			_items = new ConcurrentDictionary<object, object>();
			For = projection;
		}

		public Type For { get; }

		public void Apply(IEvent @event)
		{
			var aggregate = _items.GetOrAdd(@event.AggregateID, key => _createBlank());
//			if (_items.TryGetValue(@event.AggregateID, out aggregate) == false)
//				_items[@event.AggregateID] = aggregate = _createBlank();

			@event.Apply(aggregate, _aggregator);
		}

		public Dictionary<object, object> ToMemento() => _items.ToDictionary(x => x.Key, x => x.Value);
		public void FromMemento(Dictionary<object, object> items) => _items = new ConcurrentDictionary<object, object>(items);
	}
}
