using System;
using Crispin.Infrastructure.Storage;
using FileSystem;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class FileSystemStorageTests : StorageTests
	{
		public FileSystemStorageTests()
		{
			var root = Guid.NewGuid().ToString();
			var fs = new InMemoryFileSystem();
			fs.CreateDirectory(root).Wait();

			Storage = new FileSystemStorage(fs, root);
		}
	}
}
