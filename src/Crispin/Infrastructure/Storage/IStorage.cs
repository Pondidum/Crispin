using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorage
	{
		void RegisterBuilder<TAggregate>(Func<IEnumerable<Event>, TAggregate> builder)
			where TAggregate : AggregateRoot;

		void RegisterProjection(IProjection projection);

		IStorageSession BeginSession();
	}
}
