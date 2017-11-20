using System;
using Crispin.Infrastructure.Statistics;
using Crispin.Infrastructure.Statistics.Writers;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using Crispin.Rest.Infrastructure;
using FileSystem;
using StructureMap;

namespace Crispin.Rest
{
	public class CrispinRestRegistry : Registry
	{
		public CrispinRestRegistry()
		{
			Scan(a =>
			{
				a.AssemblyContainingType<Toggle>();
				a.WithDefaultConventions();
				a.Convention<CompositeDecorator<CompositeStatisticsWriter, IStatisticsWriter>>();
			});

			var store = BuildStorage();
			var statsStore = BuildStatisticsStore();

			For<IStorage>().Use(store);
			For<IStatisticsStore>().Use(statsStore);
			For<Func<DateTime>>().Use<Func<DateTime>>(() => () => DateTime.UtcNow);
		}

		private static IStorage BuildStorage()
		{
			var path = "../../storage";
			var fs = new PhysicalFileSystem();
			fs.CreateDirectory(path).Wait();
			
			var store = new FileSystemStorage(fs, path);
			store.RegisterProjection(new AllToggles());
			store.RegisterBuilder(Toggle.LoadFrom);

			return store;
		}

		private static IStatisticsStore BuildStatisticsStore()
		{
			var path = "../../stats";
			var fs = new PhysicalFileSystem();
			fs.CreateDirectory(path).Wait();

			var store = new  FileSystemStatisticsStore(fs, path);

			return store;
		}
	}
}
