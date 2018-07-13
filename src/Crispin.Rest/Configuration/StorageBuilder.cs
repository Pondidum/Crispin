using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using FileSystem;

namespace Crispin.Rest.Configuration
{
	public class StorageBuilder
	{
		public static IStorage Build(CrispinConfiguration config)
		{
			var path = "../../storage";
			var fs = new PhysicalFileSystem();
			fs.CreateDirectory(path).Wait();

			return Configure(new FileSystemStorage(fs, path));
		}

		public static TStore Configure<TStore>(TStore store) where TStore : IStorage
		{
			store.RegisterProjection(new AllTogglesProjection());
			store.RegisterBuilder(Toggle.LoadFrom);

			return store;
		}
	}
}
