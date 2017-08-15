using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorage
	{
		void RegisterBuilder<TAggregate>(Func<List<Event>, TAggregate> builder)
			where TAggregate : AggregateRoot;

		void RegisterProjection(Projection projection);

		IStorageSession BeginSession();
	}
}
