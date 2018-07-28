using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public class InMemorySession : IStorageSession
	{
		private readonly IDictionary<Type, Func<IEnumerable<IAct>, AggregateRoot>> _builders;
		private readonly IEnumerable<Projector> _projections;
		private readonly IDictionary<ToggleID, List<IAct>> _storeEvents;
		private readonly Dictionary<ToggleID, List<IAct>> _pendingEvents;

		public InMemorySession(
			IDictionary<Type, Func<IEnumerable<IAct>, AggregateRoot>> builders,
			IEnumerable<Projector> projections,
			IDictionary<ToggleID, List<IAct>> storeEvents)
		{
			_builders = builders;
			_projections = projections;
			_storeEvents = storeEvents;
			_pendingEvents = new Dictionary<ToggleID, List<IAct>>();
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

		public Task<TAggregate> LoadAggregate<TAggregate>(ToggleID aggregateID)
			where TAggregate : AggregateRoot
		{
			Func<IEnumerable<IAct>, AggregateRoot> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new BuilderNotFoundException(_builders.Keys, typeof(TAggregate));

			var eventsToLoad = new List<IAct>();

			if (_storeEvents.ContainsKey(aggregateID))
				eventsToLoad.AddRange(_storeEvents[aggregateID]);

			if (_pendingEvents.ContainsKey(aggregateID))
				eventsToLoad.AddRange(_pendingEvents[aggregateID]);

			if (eventsToLoad.Any() == false)
				throw new AggregateNotFoundException(typeof(TAggregate), aggregateID);

			var aggregate = builder(eventsToLoad);

			return Task.FromResult((TAggregate)aggregate);
		}


		public Task Save<TAggregate>(TAggregate aggregate)
			where TAggregate : AggregateRoot, IEvented
		{
			if (_pendingEvents.ContainsKey(aggregate.ID) == false)
				_pendingEvents.Add(aggregate.ID, new List<IAct>());

			_pendingEvents[aggregate.ID].AddRange(aggregate.GetPendingEvents());

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
			foreach (var pair in _pendingEvents)
			{
				if (_storeEvents.ContainsKey(pair.Key) == false)
					_storeEvents.Add(pair.Key, new List<IAct>());

				_storeEvents[pair.Key].AddRange(pair.Value);
			}

			var eventsForProjection = _pendingEvents
				.SelectMany(p => p.Value)
				.ToArray();

			foreach (var projection in _projections)
			foreach (var @event in eventsForProjection)
				projection.Apply(@event);

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
