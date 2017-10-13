using Crispin.Infrastructure.Storage;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class InMemoryStorageTests : StorageTests
	{
		public InMemoryStorageTests()
		{
			Storage = new InMemoryStorage();
		}
	}
}
