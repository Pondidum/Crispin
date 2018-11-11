using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Alba;
using Crispin.Conditions;
using Xunit;

namespace Crispin.Rest.Tests.Integration
{
	public class ToggleConditions : IClassFixture<ApiFixture>, IAsyncLifetime
	{
		private Toggle _toggle;
		private readonly ApiFixture _fixture;

		public ToggleConditions(ApiFixture fixture)
		{
			_fixture = fixture;
		}

		public async Task InitializeAsync()
		{
			_toggle = Toggle.CreateNew(_fixture.Editor, _fixture.Generate("toggle-conditions"));
			_toggle.AddTag(_fixture.Editor, "readonly");

			await _fixture.SaveToggle(_toggle);
		}

		public Task DisposeAsync() => Task.CompletedTask;

		private static Dictionary<string, object> Condition(string type) => new Dictionary<string, object>
		{
			{ ConditionBuilder.TypeKey, type }
		};

		[Fact]
		public Task When_adding_a_condition_to_a_toggle() => _fixture.Scenario(_ =>
		{
			_.Post
				.Json(new { Type = "enabled" })
				.ToUrl($"/api/toggles/id/{_toggle.ID}/conditions");

			_.StatusCodeShouldBe(HttpStatusCode.Created);
			_.Header("location").SingleValueShouldEqual($"/api/toggles/id/{_toggle.ID}/conditions/0");

			_.ContentShouldBe(@"{""conditionType"":""Enabled"",""id"":0}");
			_.ContentTypeShouldBe("application/json; charset=utf-8");
		});

		[Fact]
		public async Task When_removing_a_condition_from_a_toggle()
		{
			_toggle.AddCondition(EditorID.Parse("me"), Condition("disabled"));
			await _fixture.SaveToggle(_toggle);

			await _fixture.Scenario(_ =>
			{
				_.Delete
					.Url($"/api/toggles/id/{_toggle.ID}/conditions/0");

				_.StatusCodeShouldBeOk();
				_.ContentShouldBe("[]");
			});
		}

		[Fact]
		public Task When_removing_a_non_existing_condition_from_a_toggle() => _fixture.Scenario(_ =>
		{
			_.Delete
				.Url($"/api/toggles/id/{_toggle.ID}/conditions/0");

			_.StatusCodeShouldBe(HttpStatusCode.NotFound);
		});

		[Fact]
		public async Task When_a_condition_has_children()
		{
			var editor = EditorID.Parse("me");
			_toggle.AddCondition(editor, Condition("all"));
			_toggle.AddCondition(editor, Condition("enabled"), ConditionID.Parse(0));
			_toggle.AddCondition(editor, Condition("disabled"), ConditionID.Parse(0));

			await _fixture.SaveToggle(_toggle);

			await _fixture.Scenario(_ =>
			{
				_.Get
					.Url($"/api/toggles/id/{_toggle.ID}/conditions");

				_.StatusCodeShouldBeOk();
				_.ContentShouldBe(@"[
  {
    ""children"": [
      { ""conditionType"": ""Enabled"", ""id"": 1 },
      { ""conditionType"": ""Disabled"", ""id"": 2 }
    ],
    ""conditionType"": ""All"",
    ""id"": 0
  }
]".Replace(" ", "").Replace("\r\n", "").Replace("\n", ""));
			});
		}
	}
}
