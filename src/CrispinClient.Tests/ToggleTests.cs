using System;
using System.Linq;
using CrispinClient.Conditions;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class ToggleTests
	{
		[Fact]
		public void It_can_deserialize_from_the_api()
		{
			var singleToggleJson = @"{
  ""id"": ""57F137CA-4251-4D2D-BD40-EC798854593E"",
  ""conditions"": [
  {
    ""children"": [
      { ""conditionType"": ""Enabled"", ""id"": 1 },
      { ""conditionType"": ""Disabled"", ""id"": 2 }
    ],
    ""conditionType"": ""All"",
    ""id"": 0
  }]
}";

			var toggle = JsonConvert.DeserializeObject<Toggle>(singleToggleJson);

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(Guid.Parse("57F137CA-4251-4D2D-BD40-EC798854593E")),
				() => toggle.Conditions[0].ShouldBeOfType<AllCondition>(),
				() => toggle.Conditions[0].Children.First().ShouldBeOfType<EnabledCondition>(),
				() => toggle.Conditions[0].Children.Last().ShouldBeOfType<DisabledCondition>()
			);
		}
	}
}
