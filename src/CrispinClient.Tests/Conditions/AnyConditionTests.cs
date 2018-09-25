using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class AnyConditionTests : ConditionTests<AnyCondition>
	{
		[Theory]
		[MemberData(nameof(Indexes))]
		public void When_any_are_true(int trueIndex)
		{
			for (int i = 0; i < ChildConditions.Length; i++)
				ChildConditions[i].IsMatch(Reporter, Arg.Any<IToggleContext>()).Returns(i == trueIndex);

			Sut.IsMatch(Reporter, Substitute.For<IToggleContext>()).ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_true_match_are_not_evaluated(int trueIndex)
		{
			for (int i = 0; i < ChildConditions.Length; i++)
				ChildConditions[i].IsMatch(Reporter, Arg.Any<IToggleContext>()).Returns(i == trueIndex);

			Sut.IsMatch(Reporter, Substitute.For<IToggleContext>()).ShouldBeTrue();

			for (int i = trueIndex + 1; i < ChildConditions.Length; i++)
				ChildConditions[i].DidNotReceive().IsMatch(Reporter, Arg.Any<IToggleContext>());
		}

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""any"",
				""children"":[
					{""id"": 1, ""conditionType"":""enabled"" },
					{""id"": 2, ""conditionType"":""disabled"" },
				],
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<AnyCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.Children[0].ShouldBeOfType<EnabledCondition>(),
				() => condition.Children[1].ShouldBeOfType<DisabledCondition>()
			);
		}
	}
}
