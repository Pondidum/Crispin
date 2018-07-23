using System;
using System.Collections.Generic;
using FileSystem;

namespace Crispin.Infrastructure.Storage
{
	public class FileSystemStorage : IStorage
	{
		private readonly IFileSystem _fileSystem;
		private readonly string _root;
		private readonly List<IProjection> _projections;
		private readonly Dictionary<Type, Func<IEnumerable<IEvent>, AggregateRoot>> _builders;

		public FileSystemStorage(IFileSystem fileSystem, string root)
		{
			_fileSystem = fileSystem;
			_root = root;

			_builders = new Dictionary<Type, Func<IEnumerable<IEvent>, AggregateRoot>>();
			_projections = new List<IProjection>();
		}

		public void RegisterAggregate<TAggregate>() where TAggregate : AggregateRoot, new()
		{
			RegisterAggregate(() => new TAggregate());
		}

		public void RegisterAggregate<TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot
		{
			_builders[typeof(TAggregate)] = events =>
			{
				var instance = createBlank();
				var applicator = new Aggregator(typeof(TAggregate));
				applicator.Apply(instance, events);
				return instance;
			};
		}

		public void RegisterProjection(IProjection projection)
		{
			_projections.Add(projection);
		}

		public IStorageSession CreateSession() => new FileSystemSession(
			_fileSystem,
			_builders,
			_projections,
			_root);
	}
}
