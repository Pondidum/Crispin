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

		public void RegisterAggregate<TAggregate>() where TAggregate : AggregateRoot, new()
		{
			RegisterAggregate(() => new TAggregate());
		}

		public void RegisterAggregate<TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot
		{
			_builders[typeof(TAggregate)] = events =>
			{
				var instance = createBlank();
				var applicator = new Aggregator(typeof(TAggregate));
				applicator.Apply(instance, events);
				return instance;
			};
		}

		public void RegisterProjection(IProjection projection)
		{
			_projections.Add(projection);
		}

		public IStorageSession CreateSession() => new InMemorySession(
			_builders,
			_projections,
			_events);
	}
}
