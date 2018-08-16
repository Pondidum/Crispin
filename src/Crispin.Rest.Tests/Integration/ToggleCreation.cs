using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alba;
using Crispin.Rest.Tests.TestUtils;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class ToggleCreation : IntegrationBase
	{
		private const string ToggleName = "toggle-create";
		private const string ToggleDescription = "a test toggle";

		private async Task<string> CreateToggle(string name = ToggleName)
		{
			var response = await _system.Scenario(_ =>
			{
				_.Post
					.Json(new { Name = name, Description = ToggleDescription })
					.ToUrl("/toggles");

				_.StatusCodeShouldBe(HttpStatusCode.Created);
				_.HeaderShouldMatch("location", new Regex($"/toggles/id/{Regexes.Guid}$"));
			});

			return response.Context.Response.Headers["location"].Single();
		}

		[Fact]
		public Task Fetching_a_non_existing_toggle_by_id_returns_not_found() => _system.Scenario(_ =>
		{
			_.Get.Url("/toggles/id/" + Guid.NewGuid());

			_.StatusCodeShouldBe(HttpStatusCode.NotFound);
		});

		[Fact]
		public Task Fetching_a_non_existing_toggle_by_name_returns_not_found() => _system.Scenario(_ =>
		{
			_.Get.Url("/toggles/name/whaaaaat");

			_.StatusCodeShouldBe(HttpStatusCode.NotFound);
		});

		[Fact]
		public async Task Once_created_a_toggle_can_be_fetched_by_id()
		{
			var location = await CreateToggle();
			var getResponse = await _system.Scenario(_ => _.Get.Url(location));
			var toggle = getResponse.ResponseBody.ReadAsJson<Dictionary<string, object>>();

			ValidateToggle(toggle, Regex.Match(location, Regexes.Guid).Value);
		}

		[Fact]
		public async Task Once_created_a_toggle_can_be_fetched_by_name()
		{
			var location = await CreateToggle();
			var getResponse = await _system.Scenario(_ => _.Get.Url("/toggles/name/" + ToggleName));
			var toggle = getResponse.ResponseBody.ReadAsJson<Dictionary<string, object>>();

			ValidateToggle(toggle, Regex.Match(location, Regexes.Guid).Value);
		}

		[Fact]
		public async Task Once_created_the_toggle_is_in_the_main_list()
		{
			var location = await CreateToggle();
			var response = await _system.Scenario(_ => _.Get.Url("/toggles"));
			var expectedID = Regex.Match(location, Regexes.Guid).Value;

			var toggles = response.ResponseBody.ReadAsJson<Dictionary<string, object>[]>();
			var toggle = toggles.Single(x => (string)x["id"] == expectedID);

			ValidateToggle(toggle, expectedID);
		}

		[Fact]
		public async Task Creating_multiple_toggles_lists_them_separately()
		{
			var one = Regex.Match(await CreateToggle("1"), Regexes.Guid).Value;
			var two = Regex.Match(await CreateToggle("2"), Regexes.Guid).Value;
			var three = Regex.Match(await CreateToggle("3"), Regexes.Guid).Value;

			await _system.Scenario(_ =>
			{
				_.Get.Url("/toggles");

				_.ContentShouldContain(one);
				_.ContentShouldContain(two);
				_.ContentShouldContain(three);
			});
		}

		private static void ValidateToggle(Dictionary<string, object> toggle, string expectedID)
		{
			toggle.ShouldSatisfyAllConditions(
				() => toggle.ShouldContainKeyAndValue("id", expectedID),
				() => toggle.ShouldContainKeyAndValue("name", ToggleName),
				() => toggle.ShouldContainKeyAndValue("description", ToggleDescription)
			);
		}
	}
}
