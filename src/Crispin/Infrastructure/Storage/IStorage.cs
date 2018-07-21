using System;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorage
	{
		void RegisterAggregate<TAggregate>() where TAggregate : AggregateRoot, new();
		void RegisterAggregate<TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot;

		void RegisterProjection(IProjection projection);

		IStorageSession CreateSession();
	}
}
