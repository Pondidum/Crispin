using System;
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
			});

			var store = BuildStorage();
			For<IStorage>().Use(store);
			For<Func<DateTime>>().Use<Func<DateTime>>(() => () => DateTime.UtcNow);
		}

		private static IStorage BuildStorage()
		{
			var path = "../../storage";
			var fs = new PhysicalFileSystem();
			fs.CreateDirectory(path).Wait();
			
			var store = new FileSystemStorage(fs, path);
			store.RegisterProjection(new AllTogglesProjection());
			store.RegisterBuilder(Toggle.LoadFrom);

			return store;
		}
	}
}
