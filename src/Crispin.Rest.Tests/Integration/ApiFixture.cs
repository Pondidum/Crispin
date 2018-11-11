using System;
using System.Threading.Tasks;
using Alba;
using AutoFixture;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Crispin.Rest.Tests.Integration
{
	public class ApiFixture : IDisposable
	{
		private readonly SystemUnderTest _system;
		private readonly InMemoryStorage _storage;
		private readonly Fixture _fixture;

		public ApiFixture()
		{
			_fixture = new Fixture();
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

			Editor = EditorID.Parse("me");
		}

		public EditorID Editor { get; }

		public string Generate(string prefix) => _fixture.Create(prefix);

		public Task<IScenarioResult> Scenario(Action<Scenario> configure) => _system.Scenario(configure);

		public async Task SaveToggle(Toggle toggle)
		{
			using (var session = _storage.CreateSession())
				await session.Save(toggle);
		}

		public void Dispose()
		{
			_system.Dispose();
		}
	}
}
