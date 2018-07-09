using System;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorageSession : IDisposable
	{
		Task Open();

		Task<TProjection> LoadProjection<TProjection>()
			where TProjection : IProjection;

		Task<TAggregate> LoadAggregate<TAggregate>(ToggleID aggregateID)
			where TAggregate : AggregateRoot;

		Task Save<TAggregate>(TAggregate aggregate)
			where TAggregate: AggregateRoot, IEvented;

		Task Commit();
		Task Abort();
	}
}
