using System;
using System.Net;
using System.Threading.Tasks;
using Alba;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class ToggleTags : IntegrationBase
	{
		[Fact]
		public Task When_adding_a_new_tag_to_a_toggle() => _system.Scenario(_ =>
		{
			_.Put.Url($"/api/toggles/id/{_toggle.ID}/tags/env:test");
			_.StatusCodeShouldBe(HttpStatusCode.OK);
			_.ContentShouldBe("[\"readonly\",\"env:test\"]");
		});

		[Fact]
		public Task When_adding_an_existing_tag_to_a_toggle() => _system.Scenario(_ =>
		{
			_.Put.Url($"/api/toggles/id/{_toggle.ID}/tags/readonly");
			_.StatusCodeShouldBe(HttpStatusCode.OK);
			_.ContentShouldBe("[\"readonly\"]");
		});

		[Fact]
		public Task When_adding_a_tag_to_a_non_existing_toggle() => _system.Scenario(_ =>
		{
			_.Put.Url($"/api/toggles/id/{Guid.NewGuid()}/tags/env:test");
			_.StatusCodeShouldBe(HttpStatusCode.NotFound);
		});

		[Fact]
		public Task When_removing_an_existing_tag_to_a_toggle() => _system.Scenario(_ =>
		{
			_.Delete.Url($"/api/toggles/id/{_toggle.ID}/tags/readonly");
			_.StatusCodeShouldBe(HttpStatusCode.OK);
			_.ContentShouldBe("[]");
		});

		[Fact]
		public Task When_removing_a_non_existing_tag_from_a_toggle() => _system.Scenario(_ =>
		{
			_.Delete.Url($"/api/toggles/id/{_toggle.ID}/tags/env:test");
			_.StatusCodeShouldBe(HttpStatusCode.OK);
			_.ContentShouldBe("[\"readonly\"]");
		});

		[Fact]
		public Task When_removing_a_tag_from_a_non_existing_toggle() => _system.Scenario(_ =>
		{
			_.Delete.Url($"/api/toggles/id/{Guid.NewGuid()}/tags/env:test");
			_.StatusCodeShouldBe(HttpStatusCode.NotFound);
		});
	}
}
