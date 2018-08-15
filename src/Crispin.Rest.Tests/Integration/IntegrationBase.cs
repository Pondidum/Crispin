using System.Threading.Tasks;
using Alba;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class IntegrationBase : IAsyncLifetime
	{
		protected readonly SystemUnderTest _system;
		protected readonly InMemoryStorage _storage;

		public IntegrationBase()
		{
			_storage = new InMemoryStorage();
			_storage.RegisterProjection<ToggleView>();
			_storage.RegisterAggregate<ToggleID, Toggle>();

			_system = SystemUnderTest.ForStartup<Startup>();
			_system.Configure(builder => builder.UseLamar());
			_system.ConfigureServices(services => services.AddSingleton<IStorage>(_storage));
		}

		public virtual Task InitializeAsync() => Task.CompletedTask;

		public Task DisposeAsync() => Task.Run(() => _system.Dispose());
	}
}
