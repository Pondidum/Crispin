using System;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorage
	{
		void RegisterAggregate<TAggregate>() where TAggregate : AggregateRoot, new();
		void RegisterAggregate<TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot;

		void RegisterProjection<TProjection>() where TProjection : new();
		void RegisterProjection<TProjection>(Func<TProjection> createBlank);

		IStorageSession CreateSession();
	}
}
