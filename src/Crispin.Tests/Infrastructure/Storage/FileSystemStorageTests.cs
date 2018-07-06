using System;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using FileSystem;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class FileSystemStorageTests : StorageTests
	{
		protected override async Task<IStorage> CreateStorage()
		{
			var root = Guid.NewGuid().ToString();
			var fs = new InMemoryFileSystem();

			await fs.CreateDirectory(root);

			return new FileSystemStorage(fs, root);
		}
	}
}
