using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public class PendingEventsStore
	{
		private readonly Dictionary<object, List<IEvent>> _pending;

		public PendingEventsStore()
		{
			_pending = new Dictionary<object, List<IEvent>>();
		}

		public IEnumerable<IEvent> AllEvents => _pending.SelectMany(pair => pair.Value);

		public bool Any() => _pending.Any();
		public void Clear() => _pending.Clear();

		public IEnumerable<IEvent> EventsFor<T>(T aggregateID) => _pending.ContainsKey(aggregateID)
			? _pending[aggregateID]
			: Enumerable.Empty<IEvent>();

		public void AddEvents(IEnumerable<IEvent> pending)
		{
			pending.Each(AddEvent);
		}

		public void AddEvent(IEvent pending)
		{
			if (_pending.ContainsKey(pending.AggregateID) == false)
				_pending[pending.AggregateID] = new List<IEvent>();

			_pending[pending.AggregateID].Add(pending);
		}

		public async Task ForEach(Func<object, IEnumerable<IEvent>, Task> action)
		{
			foreach (var pair in _pending)
				await action(pair.Key, pair.Value);
		}
	}
}
