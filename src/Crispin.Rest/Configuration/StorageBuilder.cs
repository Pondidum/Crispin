using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using FileSystem;

namespace Crispin.Rest.Configuration
{
	public class StorageBuilder
	{
		public static IStorage Build()
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
