using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public class InMemorySession : IStorageSession
	{
		private readonly IDictionary<Type, Func<IEnumerable<IEvent>, object>> _builders;
		private readonly IEnumerable<Projector> _projections;
		private readonly IDictionary<object, List<IEvent>> _storeEvents;
		private readonly PendingEventsStore _pendingEvents;

		public InMemorySession(
			IDictionary<Type, Func<IEnumerable<IEvent>, object>> builders,
			IEnumerable<Projector> projections,
			IDictionary<object, List<IEvent>> storeEvents)
		{
			_builders = builders;
			_projections = projections;
			_storeEvents = storeEvents;
			_pendingEvents = new PendingEventsStore();
		}

		public Task Open() => Task.CompletedTask;

		public Task<IEnumerable<TProjection>> QueryProjection<TProjection>()
		{
			var projection = _projections
				.FirstOrDefault(p => p.For == typeof(TProjection));

			if (projection != null)
				return Task.FromResult(projection.ToMemento().Values.Cast<TProjection>());

			throw new ProjectionNotRegisteredException(typeof(TProjection).Name);
		}

		public Task<TAggregate> LoadAggregate<TAggregate>(object aggregateID)
		{
			Func<IEnumerable<IEvent>, object> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new BuilderNotFoundException(_builders.Keys, typeof(TAggregate));

			var eventsToLoad = new List<IEvent>();

			if (_storeEvents.ContainsKey(aggregateID))
				eventsToLoad.AddRange(_storeEvents[aggregateID]);

			eventsToLoad.AddRange(_pendingEvents.EventsFor<TAggregate>(aggregateID));

			if (eventsToLoad.Any() == false)
				throw new AggregateNotFoundException(typeof(TAggregate), aggregateID);

			var aggregate = builder(eventsToLoad);

			return Task.FromResult((TAggregate)aggregate);
		}


		public Task Save<TAggregate>(TAggregate aggregate) where TAggregate : IEvented
		{
			_pendingEvents.AddEvents<TAggregate>(aggregate.GetPendingEvents());

			aggregate.ClearPendingEvents();

			return Task.CompletedTask;
		}

		public Task Commit()
		{
			PerformCommit();

			return Task.CompletedTask;
		}

		public Task Abort()
		{
			_pendingEvents.Clear();
			return Task.CompletedTask;
		}

		private void PerformCommit()
		{
			foreach (var @event in _pendingEvents.AllEvents)
			{
				if (_storeEvents.ContainsKey(@event.AggregateID) == false)
					_storeEvents.Add(@event.AggregateID, new List<IEvent>());

				_storeEvents[@event.AggregateID].Add(@event);

				foreach (var projection in _projections)
					projection.Apply(@event);
			}

			_pendingEvents.Clear();
		}

		public void Dispose()
		{
			if (_pendingEvents.Any() == false)
				return;

			PerformCommit();
		}
	}
}
