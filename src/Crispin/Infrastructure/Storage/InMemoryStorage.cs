using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Storage
{
	public class InMemoryStorage : IStorage
	{
		private readonly IDictionary<Type, Func<IEnumerable<IEvent>, object>> _builders;
		private readonly PendingEventsStore _events;
		private readonly Dictionary<Type, Projector> _projections;

		public InMemoryStorage(PendingEventsStore events = null)
		{
			_builders = new Dictionary<Type, Func<IEnumerable<IEvent>, object>>();
			_events = events ?? new PendingEventsStore();
			_projections = new Dictionary<Type, Projector>();
		}

		public void RegisterAggregate<TIdentity, TAggregate>() where TAggregate : AggregateRoot<TIdentity>, new()
		{
			RegisterAggregate<TIdentity, TAggregate>(() => new TAggregate());
		}

		public void RegisterAggregate<TIdentity, TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot<TIdentity>
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
