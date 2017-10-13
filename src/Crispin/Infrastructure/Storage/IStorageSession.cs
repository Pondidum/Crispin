using System;
using System.Threading.Tasks;

namespace Crispin.Infrastructure.Storage
{
	public interface IStorageSession : IDisposable
	{
		Task Open();

		TProjection LoadProjection<TProjection>()
			where TProjection : IProjection;

		TAggregate LoadAggregate<TAggregate>(ToggleID aggregateID)
			where TAggregate : AggregateRoot;

		void Save<TAggregate>(TAggregate aggregate)
			where TAggregate: AggregateRoot, IEvented;

		Task Commit();
	}
}
