using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class InMemoryStorageTests : StorageTests
	{
		protected override Task<IStorage> CreateStorage() => Task.FromResult((IStorage)new InMemoryStorage());
	}
}
