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
		private readonly Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> _builders;

		public FileSystemStorage(IFileSystem fileSystem, string root)
		{
			_fileSystem = fileSystem;
			_root = root;

			_builders = new Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>>();
			_projections = new List<IProjection>();
		}

		public void RegisterBuilder<TAggregate>(Func<IEnumerable<Event>, TAggregate> builder) where TAggregate : AggregateRoot
		{
			_builders.Add(typeof(TAggregate), builder);
		}

		public void RegisterProjection(IProjection projection)
		{
			_projections.Add(projection);
		}

		public IStorageSession BeginSession()
		{
			return new FileSystemSession(_fileSystem, _builders, _projections, _root);
		}
	}
}
