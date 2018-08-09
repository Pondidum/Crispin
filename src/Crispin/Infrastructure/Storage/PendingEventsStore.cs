using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public class PendingEventsStore
	{
		private readonly LightweightCache<Type, LightweightCache<object, List<IEvent>>> _pending;

		public PendingEventsStore()
		{
			_pending = new LightweightCache<Type, LightweightCache<object, List<IEvent>>>(
				type => new LightweightCache<object, List<IEvent>>(
					id => new List<IEvent>()
				)
			);
		}

		public IEnumerable<IEvent> AllEvents => _pending.GetAll().SelectMany(types => types.GetAll().SelectMany(events => events));

		public bool Any() => _pending.Any();
		public void Clear() => _pending.Clear();

		public IEnumerable<IEvent> EventsFor<TAggregate>(object aggregateID) => _pending[typeof(TAggregate)][aggregateID];

		public void AddEvents<TAggregate>(IEnumerable<IEvent> pending)
		{
			foreach (var @event in pending)
				AddEvent<TAggregate>(@event);
		}

		public void AddEvent<TAggregate>(IEvent pending)
		{
			_pending[typeof(TAggregate)][pending.AggregateID].Add(pending);
		}

		public async Task ForEach(Func<Type, object, IEnumerable<IEvent>, Task> action)
		{
			foreach (var aggregateType in _pending)
			foreach (var aggregateEvents in aggregateType.Value)
				await action(aggregateType.Key, aggregateEvents.Key, aggregateEvents.Value);
		}
	}
}
