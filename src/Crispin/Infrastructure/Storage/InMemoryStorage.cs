using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Storage
{
	public class InMemoryStorage : IStorage
	{
		private readonly IDictionary<Type, Func<IEnumerable<IEvent>, AggregateRoot>> _builders;
		private readonly IDictionary<ToggleID, List<IEvent>> _events;
		private readonly Dictionary<Type, Projector> _projections;

		public InMemoryStorage(Dictionary<ToggleID, List<IEvent>> events = null)
		{
			_builders = new Dictionary<Type, Func<IEnumerable<IEvent>, AggregateRoot>>();
			_events = events ?? new Dictionary<ToggleID, List<IEvent>>();
			_projections = new Dictionary<Type, Projector>();
		}

		public void RegisterAggregate<TAggregate>() where TAggregate : AggregateRoot, new()
		{
			RegisterAggregate(() => new TAggregate());
		}

		public void RegisterAggregate<TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot
		{
			_builders[typeof(TAggregate)] = events => AggregateBuilder.Build(createBlank, events);
		}

		public void RegisterProjection<TProjection>() where TProjection : new()
		{
			RegisterProjection(() => new TProjection());
		}

		public void RegisterProjection<TProjection>(Func<TProjection> createBlank)
		{
			_projections.Add(typeof(TProjection), new Projector(typeof(TProjection), () => createBlank()));
		}

		public IStorageSession CreateSession() => new InMemorySession(
			_builders,
			_projections.Values,
			_events);
	}
}
