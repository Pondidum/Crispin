using System;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
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

			For<Func<DateTime>>().Use<Func<DateTime>>(() => () => DateTime.UtcNow);

			For<IStorage>().UseIfNone(() => BuildStorage());
			For<IStorageSession>().Use(c => c.GetInstance<IStorage>().BeginSession().Result);
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
