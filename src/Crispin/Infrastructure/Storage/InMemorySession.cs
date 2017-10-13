using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public class InMemorySession : IStorageSession
	{
		private readonly IDictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> _builders;
		private readonly List<IProjection> _projections;
		private readonly IDictionary<ToggleID, List<Event>> _storeEvents;
		private readonly Dictionary<ToggleID, List<Event>> _pendingEvents;

		public InMemorySession(
			IDictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> builders,
			List<IProjection> projections,
			IDictionary<ToggleID, List<Event>> storeEvents)
		{
			_builders = builders;
			_projections = projections;
			_storeEvents = storeEvents;
			_pendingEvents = new Dictionary<ToggleID, List<Event>>();
		}

		public Task Open() => Task.CompletedTask;

		public Task<TProjection> LoadProjection<TProjection>()
			where TProjection : IProjection
		{
			var projection = _projections
				.OfType<TProjection>()
				.FirstOrDefault();

			if (projection != null)
				return Task.FromResult(projection);

			throw new ProjectionNotRegisteredException(typeof(TProjection).Name);
		}

		public Task<TAggregate> LoadAggregate<TAggregate>(ToggleID aggregateID)
			where TAggregate : AggregateRoot
		{
			Func<IEnumerable<Event>, AggregateRoot> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new NotSupportedException($"No builder for type {typeof(TAggregate).Name} found.");

			var eventsToLoad = new List<Event>();

			if (_storeEvents.ContainsKey(aggregateID))
				eventsToLoad.AddRange(_storeEvents[aggregateID]);

			if (_pendingEvents.ContainsKey(aggregateID))
				eventsToLoad.AddRange(_pendingEvents[aggregateID]);

			if (eventsToLoad.Any() == false)
				throw new KeyNotFoundException($"Unable to find an aggregate with ID {aggregateID}");

			var aggregate = builder(eventsToLoad);

			return Task.FromResult((TAggregate)aggregate);
		}


		public Task Save<TAggregate>(TAggregate aggregate)
			where TAggregate : AggregateRoot, IEvented
		{
			if (_pendingEvents.ContainsKey(aggregate.ID) == false)
				_pendingEvents.Add(aggregate.ID, new List<Event>());

			_pendingEvents[aggregate.ID].AddRange(aggregate.GetPendingEvents().Cast<Event>());

			aggregate.ClearPendingEvents();

			return Task.CompletedTask;
		}

		public Task Commit()
		{
			foreach (var pair in _pendingEvents)
			{
				if (_storeEvents.ContainsKey(pair.Key) == false)
					_storeEvents.Add(pair.Key, new List<Event>());

				_storeEvents[pair.Key].AddRange(pair.Value);
			}

			var eventsForProjection = _pendingEvents
				.SelectMany(p => p.Value)
				.ToArray();

			foreach (var projection in _projections)
			foreach (var @event in eventsForProjection)
				projection.Consume(@event);

			_pendingEvents.Clear();

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			if (_pendingEvents.Any() == false)
				return;

			Commit().Wait();
		}
	}
}
