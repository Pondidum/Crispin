using System;
using System.Collections.Generic;

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
			List<Event> eventsToLoad;

			if (_events.TryGetValue(aggregateID, out eventsToLoad) == false)
				throw new KeyNotFoundException($"Unable to find an aggregate with ID {aggregateID}");

			Func<List<Event>, AggregateRoot> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new NotSupportedException($"No builder for type {typeof(TAggregate).Name} found.");

			var aggregate = builder(eventsToLoad);

			return (TAggregate)aggregate;
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
