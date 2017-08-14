using System;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorageSession : IDisposable
	{
		void Open();

		TAggregate LoadAggregate<TAggregate>(Guid aggregateID)
			where TAggregate : AggregateRoot;

		void Commit();
		void Close();
	}
}
