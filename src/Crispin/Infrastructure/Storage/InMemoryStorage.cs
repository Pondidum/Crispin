using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Storage
{
	public class InMemoryStorage : IStorage
	{
		private readonly IDictionary<Type, Func<List<Event>, AggregateRoot>> _builders;
		private readonly IDictionary<ToggleID, List<Event>> _events;
		private readonly List<Projection> _projections;

		public InMemoryStorage(Dictionary<ToggleID, List<Event>> events = null)
		{
			_builders = new Dictionary<Type, Func<List<Event>, AggregateRoot>>();
			_events = events ?? new Dictionary<ToggleID, List<Event>>();
			_projections = new List<Projection>();
		}

		public void RegisterBuilder<TAggregate>(Func<List<Event>, TAggregate> builder)
			where TAggregate : AggregateRoot
		{
			_builders.Add(typeof(TAggregate), builder);
		}

		public void RegisterProjection(Projection projection)
		{
			_projections.Add(projection);
		}

		public IStorageSession BeginSession()
		{
			var session = new InMemorySession(_builders, _projections, _events);
			session.Open();

			return session;
		}
	}
}
