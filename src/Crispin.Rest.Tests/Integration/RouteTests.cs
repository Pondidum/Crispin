using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Crispin.Conditions;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class RouteTests : IClassFixture<ApiFixture>, IAsyncLifetime
	{
		private readonly ApiFixture _fixture;
		private Toggle _toggle;
		private const string Tag = "environment:dev";

		public RouteTests(ApiFixture fixture)
		{
			_fixture = fixture;
		}

		public async Task InitializeAsync()
		{
			_toggle = Toggle.CreateNew(_fixture.Editor, _fixture.Generate("toggle-routes"));
			_toggle.AddTag(_fixture.Editor, "readonly");
			_toggle.AddTag(_fixture.Editor, "test");
			_toggle.AddCondition(_fixture.Editor, new Dictionary<string, object>
			{
				{ ConditionBuilder.TypeKey, "enabled" }
			});

			await _fixture.SaveToggle(_toggle);
		}

		public Task DisposeAsync() => Task.CompletedTask;

		[Theory]
		[InlineData("GET", "/api/toggles")]
		//
		[InlineData("GET", "/api/toggles/id/{id}")]
		[InlineData("GET", "/api/toggles/name/{name}")]
		//
		[InlineData("GET", "/api/toggles/id/{id}/name")]
		[InlineData("GET", "/api/toggles/name/{name}/name")]
		//
		[InlineData("PUT", "/api/toggles/id/{id}/name", "{\"name\":\"wat\"}")]
		[InlineData("PUT", "/api/toggles/name/{name}/name", "{\"name\":\"wat\"}")]
		//
		[InlineData("GET", "/api/toggles/id/{id}/description")]
		[InlineData("GET", "/api/toggles/name/{name}/description")]
		//
		[InlineData("PUT", "/api/toggles/id/{id}/description", "{\"description\":\"wat\"}")]
		[InlineData("PUT", "/api/toggles/name/{name}/description", "{\"description\":\"wat\"}")]
		//
		[InlineData("GET", "/api/toggles/id/{id}/tags")]
		[InlineData("GET", "/api/toggles/name/{name}/tags")]
		//
		[InlineData("PUT", "/api/toggles/id/{id}/tags/environment:test")]
		[InlineData("PUT", "/api/toggles/name/{name}/tags/environment:test")]
		//
		[InlineData("DELETE", "/api/toggles/id/{id}/tags/" + Tag)]
		[InlineData("DELETE", "/api/toggles/name/{name}/tags/" + Tag)]
		//
		[InlineData("GET", "/api/toggles/id/{id}/conditions")]
		[InlineData("GET", "/api/toggles/name/{name}/conditions")]
		//
		[InlineData("GET", "/api/toggles/id/{id}/conditions/0")]
		[InlineData("GET", "/api/toggles/name/{name}/conditions/0")]
		public Task Route_works(string method, string url, string body = null) => _fixture.Scenario(_ =>
		{
			_.Context.HttpMethod(method);
			_.Context.RelativeUrl(url
				.Replace("{id}", _toggle.ID.ToString())
				.Replace("{name}", _toggle.Name));

			if (body != null)
				_.Body.JsonInputIs(body);

			_.StatusCodeShouldBeOk();
			_.ContentTypeShouldBe("application/json; charset=utf-8");
		});
	}
}
