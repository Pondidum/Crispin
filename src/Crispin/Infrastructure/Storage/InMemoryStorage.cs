using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public class InMemoryStorage : IStorage
	{
		private readonly IDictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> _builders;
		private readonly IDictionary<ToggleID, List<Event>> _events;
		private readonly List<IProjection> _projections;

		public InMemoryStorage(Dictionary<ToggleID, List<Event>> events = null)
		{
			_builders = new Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>>();
			_events = events ?? new Dictionary<ToggleID, List<Event>>();
			_projections = new List<IProjection>();
		}

		public void RegisterBuilder<TAggregate>(Func<IEnumerable<Event>, TAggregate> builder)
			where TAggregate : AggregateRoot
		{
			_builders.Add(typeof(TAggregate), builder);
		}

		public void RegisterProjection(IProjection projection)
		{
			_projections.Add(projection);
		}

		public IStorageSession BeginSession() => new InMemorySession(
			_builders,
			_projections,
			_events);
	}
}
