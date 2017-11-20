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

			For<IStorage>().Use(store);
			For<IStatisticsStore>().Use<FileSystemStatisticsStore>().Singleton();
			For<Func<DateTime>>().Use<Func<DateTime>>(() => () => DateTime.UtcNow);
		}

		private static IStorage BuildStorage()
		{
			var fs = new PhysicalFileSystem();
			var store = new FileSystemStorage(fs, "../../storage");
			store.RegisterProjection(new AllToggles());
			store.RegisterBuilder(Toggle.LoadFrom);

			return store;
		}
	}
}
