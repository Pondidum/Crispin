using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class EnabledConditionTests
	{
		[Fact]
		public void It_always_returns_true() => new EnabledCondition()
			.IsMatch(Substitute.For<IToggleContext>())
			.ShouldBe(true);

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""enabled""
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<EnabledCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.Children.ShouldBeEmpty()
			);
		}
	}
}
