using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alba;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using Crispin.Rest.Tests.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class Toggles : IDisposable
	{
		private readonly SystemUnderTest _system;
		private readonly InMemoryStorage _storage;

		public Toggles()
		{
			_storage = new InMemoryStorage();
			_storage.RegisterProjection(new AllTogglesProjection());
			_storage.RegisterBuilder(Toggle.LoadFrom);

			_system = SystemUnderTest.ForStartup<Startup>();
			_system.ConfigureServices(services => services.AddSingleton<IStorage>(_storage));
		}

		[Fact]
		public async Task When_listing_all_toggles()
		{
			await _system.Scenario(_ =>
			{
				_.Get.Url("/toggles/");
				_.ContentShouldBe("[]");
				_.StatusCodeShouldBeOk();
			});
		}

		[Fact]
		public async Task Creating_a_toggle_returns_correct_headers()
		{
			await _system.Scenario(_ =>
			{
				_.Post
					.Json(new { Name = "toggle-1", Description = "a test toggle" })
					.ToUrl("/toggles");

				_.StatusCodeShouldBe(HttpStatusCode.Created);
				_.HeaderShouldMatch("location", new Regex($"/toggles/id/{Regexes.Guid}$"));
			});
		}

		public void Dispose()
		{
			_system.Dispose();
		}
	}
}
