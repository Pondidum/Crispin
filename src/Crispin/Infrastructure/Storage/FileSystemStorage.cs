using System;
using System.Collections.Generic;
using FileSystem;

namespace Crispin.Infrastructure.Storage
{
	public class FileSystemStorage : IStorage
	{
		private readonly IFileSystem _fileSystem;
		private readonly string _root;
		private readonly Dictionary<Type, Projector> _projections;
		private readonly Dictionary<Type, Func<IEnumerable<IAct>, AggregateRoot>> _builders;

		public FileSystemStorage(IFileSystem fileSystem, string root)
		{
			_fileSystem = fileSystem;
			_root = root;

			_builders = new Dictionary<Type, Func<IEnumerable<IAct>, AggregateRoot>>();
			_projections = new Dictionary<Type, Projector>();
		}

		public void RegisterAggregate<TAggregate>() where TAggregate : AggregateRoot, new()
		{
			RegisterAggregate(() => new TAggregate());
		}

		public void RegisterAggregate<TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot
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

		public IStorageSession CreateSession() => new FileSystemSession(
			_fileSystem,
			_builders,
			_projections.Values,
			_root);
	}
}
