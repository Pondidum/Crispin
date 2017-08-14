using System;
using System.Collections.Generic;
using System.Linq;

namespace Crispin.Infrastructure.Storage
{
	public class InMemorySession : IStorageSession
	{
		private readonly IDictionary<Type, Func<List<Event>, AggregateRoot>> _builders;
		private readonly IDictionary<Guid, List<Event>> _events;

		public InMemorySession(
			IDictionary<Type, Func<List<Event>, AggregateRoot>> builders,
			IDictionary<Guid, List<Event>> events)
		{
			_builders = builders;
			_events = events;
		}

		public void Open()
		{
		}

		public TAggregate LoadAggregate<TAggregate>(Guid aggregateID) where TAggregate : AggregateRoot
		{
			Func<List<Event>, AggregateRoot> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new NotSupportedException($"No builder for type {typeof(TAggregate).Name} found.");

			List<Event> eventsToLoad;

			if (_events.TryGetValue(aggregateID, out eventsToLoad) == false || eventsToLoad.Any() == false)
				throw new KeyNotFoundException($"Unable to find an aggregate with ID {aggregateID}");

			var aggregate = builder(eventsToLoad);

			return (TAggregate)aggregate;
		}

		
		public void Save<TAggregate>(TAggregate aggregate)
			where TAggregate : AggregateRoot, IEvented
		{
			if (_events.ContainsKey(aggregate.ID) == false)
				_events.Add(aggregate.ID, new List<Event>());

			_events[aggregate.ID].AddRange(aggregate.GetPendingEvents().Cast<Event>());

			aggregate.ClearPendingEvents();
		}

		public void Commit()
		{
			throw new NotImplementedException();
		}

		public void Close()
		{
		}


		public void Dispose()
		{
//			if (anythingPending)
//				Commit();

			Close();
		}
	}
}
