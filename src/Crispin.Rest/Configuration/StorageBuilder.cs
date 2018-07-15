using System;
using System.Collections.Generic;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using FileSystem;

namespace Crispin.Rest.Configuration
{
	public class StorageBuilder
	{
		private static readonly Dictionary<StorageBackends, Func<CrispinConfiguration, IStorage>> Builders;

		static StorageBuilder()
		{
			Builders = new Dictionary<StorageBackends, Func<CrispinConfiguration, IStorage>>
			{
				{ StorageBackends.InMemory, BuildInMemoryStorage },
				{ StorageBackends.FileSystem, BuildFileSystemStorage }
			};
		}

		public static IStorage Build(CrispinConfiguration config)
		{
			var storage = Builders[config.Backend](config);

			return Configure(storage);
		}

		private static IStorage BuildInMemoryStorage(CrispinConfiguration config)
		{
			return new InMemoryStorage();
		}

		private static IStorage BuildFileSystemStorage(CrispinConfiguration config)
		{
			var fs = new PhysicalFileSystem();
			fs.CreateDirectory(config.ConnectionString).Wait();

			return new FileSystemStorage(fs, config.ConnectionString);
		}

		public static TStore Configure<TStore>(TStore store) where TStore : IStorage
		{
			store.RegisterProjection(new AllTogglesProjection());
			store.RegisterBuilder(Toggle.LoadFrom);

			return store;
		}
	}
}
