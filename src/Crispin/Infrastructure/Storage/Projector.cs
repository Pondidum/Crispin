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
		private ConcurrentDictionary<string, object> _items;

		public Projector(Type projection, Func<object> createBlank)
		{
			_createBlank = createBlank;
			_aggregator = new Aggregator(projection);
			_items = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			For = projection;
		}

		public Type For { get; }

		public void Apply(IEvent @event)
		{
			var aggregate = _items.GetOrAdd(@event.AggregateID.ToString(), key => _createBlank());

			@event.Apply(aggregate, _aggregator);
		}

		public Dictionary<object, object> ToMemento()
		{
			return _items.ToDictionary(x => (object)x.Key, x => x.Value);
		}

		public void FromMemento(Dictionary<object, object> items)
		{
			_items = new ConcurrentDictionary<string, object>(items.ToDictionary(x => x.Key.ToString(), x => x.Value));
		}
	}
}
