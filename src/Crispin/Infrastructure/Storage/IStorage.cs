using System;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorage
	{
		void RegisterAggregate<TIdentity, TAggregate>() where TAggregate : AggregateRoot<TIdentity>, new();
		void RegisterAggregate<TIdentity, TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot<TIdentity>;

		void RegisterProjection<TProjection>() where TProjection : new();
		void RegisterProjection<TProjection>(Func<TProjection> createBlank);

		IStorageSession CreateSession();
	}
}
