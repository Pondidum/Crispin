using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Crispin.Conditions;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class RouteTests : IntegrationBase
	{
		private const string Tag = "environment:dev";

		public RouteTests()
		{
			_toggle.AddTag(_editor, "test");
			_toggle.AddCondition(_editor, new Dictionary<string, object>
			{
				{ ConditionBuilder.TypeKey, "enabled" }
			});
		}

		[Theory]
		[InlineData("GET", "/api/toggles")]
		//
		[InlineData("GET", "/api/toggles/id/{id}")]
		[InlineData("GET", "/api/toggles/name/{name}")]
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
		public Task Route_works(string method, string url) => _system.Scenario(_ =>
		{
			url = url.Replace("{id}", _toggle.ID.ToString()).Replace("{name}", _toggle.Name);

			_.Context.HttpMethod(method);
			_.Context.RelativeUrl(url);

			_.StatusCodeShouldBeOk();
			_.ContentTypeShouldBe("application/json; charset=utf-8");
		});
	}
}
