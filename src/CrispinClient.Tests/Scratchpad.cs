using System.Linq;
using CrispinClient.Conditions;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class Scratchpad
	{
		[Fact]
		public void When_testing_something()
		{
			var singleToggleJson = @"{
  ""id"": ""57F137CA-4251-4D2D-BD40-EC798854593E"",
  ""conditions"": [
  {
    ""id"": 0,
    ""conditionType"": ""ingroup"",
    ""groupName"":""where to look"",
    ""searchKey"":""what to find""
  }]
}";

			var toggle = JsonConvert.DeserializeObject<Toggle>(singleToggleJson);
			var condition = toggle
				.Conditions
				.Single()
				.ShouldBeOfType<InGroupCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.GroupName.ShouldBe("where to look"),
				() => condition.SearchKey.ShouldBe("what to find")
			);
		}
	}
}
