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
		[InlineData("GET", "/toggles")]
		//
		[InlineData("GET", "/toggles/id/{id}")]
		[InlineData("GET", "/toggles/name/{name}")]
		//
		[InlineData("GET", "/toggles/id/{id}/tags")]
		[InlineData("GET", "/toggles/name/{name}/tags")]
		//
		[InlineData("PUT", "/toggles/id/{id}/tags/environmnet:test")]
		[InlineData("PUT", "/toggles/name/{name}/tags/environmnet:test")]
		//
		[InlineData("DELETE", "/toggles/id/{id}/tags/" + Tag)]
		[InlineData("DELETE", "/toggles/name/{name}/tags/" + Tag)]
		//
		[InlineData("GET", "/toggles/id/{id}/conditions")]
		[InlineData("GET", "/toggles/name/{name}/conditions")]
		//
		[InlineData("GET", "/toggles/id/{id}/conditions/0")]
		[InlineData("GET", "/toggles/name/{name}/conditions/0")]
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
