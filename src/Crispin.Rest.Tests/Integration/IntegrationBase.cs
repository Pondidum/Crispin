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
		protected Toggle _toggle;
		protected readonly EditorID _editor;

		public IntegrationBase()
		{
			_storage = new InMemoryStorage();
			_storage.RegisterProjection<ToggleView>();
			_storage.RegisterAggregate<ToggleID, Toggle>();

			_system = SystemUnderTest.ForStartup<Startup>();
			_system.Configure(builder => builder.UseLamar());
			_system.ConfigureServices(services =>
			{
				services.AddSingleton<IStorage>(_storage);
				services.AddScoped(sp => _storage.CreateSession());
			});

			_editor = EditorID.Parse("me");
			_toggle = Toggle.CreateNew(_editor, "toggle-1");
			_toggle.AddTag(_editor, "readonly");

		}

		public async Task InitializeAsync()
		{
			using (var session = _storage.CreateSession())
				await session.Save(_toggle);
		}

		public Task DisposeAsync() => Task.Run(() => _system.Dispose());
	}
}
