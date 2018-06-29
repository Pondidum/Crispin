using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alba;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using Crispin.Rest.Tests.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class ToggleCreation : IDisposable
	{
		private const string ToggleName = "toggle-1";
		private const string ToggleDescription = "a test toggle";
		private readonly SystemUnderTest _system;
		private readonly InMemoryStorage _storage;

		public ToggleCreation()
		{
			_storage = new InMemoryStorage();
			_storage.RegisterProjection(new AllTogglesProjection());
			_storage.RegisterBuilder(Toggle.LoadFrom);

			_system = SystemUnderTest.ForStartup<Startup>();
			_system.ConfigureServices(services => services.AddSingleton<IStorage>(_storage));
		}

		private async Task<string> CreateToggle()
		{
			var response = await _system.Scenario(_ =>
			{
				_.Post
					.Json(new { Name = ToggleName, Description = ToggleDescription })
					.ToUrl("/toggles");

				_.StatusCodeShouldBe(HttpStatusCode.Created);
				_.HeaderShouldMatch("location", new Regex($"/toggles/id/{Regexes.Guid}$"));
			});

			return response.Context.Response.Headers["location"].Single();
		}

		[Fact]
		public async Task Once_created_a_toggle_can_be_fetched_by_id()
		{
			var location = await CreateToggle();
			var getResponse = await _system.Scenario(_ => _.Get.Url(location));
			var toggle = getResponse.ResponseBody.ReadAsJson<Dictionary<string, object>>();

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ShouldContainKeyAndValue("id", Regex.Match(location, Regexes.Guid).Value),
				() => toggle.ShouldContainKeyAndValue("name", ToggleName),
				() => toggle.ShouldContainKeyAndValue("description", ToggleDescription)
			);
		}

		[Fact]
		public async Task Once_created_a_toggle_can_be_fetched_by_name()
		{
			var location = await CreateToggle();
			var getResponse = await _system.Scenario(_ => _.Get.Url("/toggles/name/" + ToggleName));
			var toggle = getResponse.ResponseBody.ReadAsJson<Dictionary<string, object>>();

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ShouldContainKeyAndValue("id", Regex.Match(location, Regexes.Guid).Value),
				() => toggle.ShouldContainKeyAndValue("name", ToggleName),
				() => toggle.ShouldContainKeyAndValue("description", ToggleDescription)
			);
		}

		public void Dispose()
		{
			_system.Dispose();
		}
	}
}
