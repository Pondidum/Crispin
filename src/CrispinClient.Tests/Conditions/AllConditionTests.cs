using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class AllConditionTests : ConditionTests<AllCondition>
	{
		[Theory]
		[MemberData(nameof(Indexes))]
		public void When_any_are_false(int falseIndex)
		{
			for (int i = 0; i < ChildConditions.Length; i++)
				ChildConditions[i].IsMatch(Stats, Arg.Any<IToggleContext>()).Returns(i != falseIndex);

			Sut.IsMatch(Stats, Substitute.For<IToggleContext>()).ShouldBeFalse();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_false_match_are_not_evaluated(int falseIndex)
		{
			for (int i = 0; i < ChildConditions.Length; i++)
				ChildConditions[i].IsMatch(Stats, Arg.Any<IToggleContext>()).Returns(i != falseIndex);

			Sut.IsMatch(Stats, Substitute.For<IToggleContext>()).ShouldBeFalse();

			for (int i = falseIndex + 1; i < ChildConditions.Length; i++)
				ChildConditions[i].DidNotReceive().IsMatch(Stats, Arg.Any<IToggleContext>());
		}

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""all"",
				""children"":[
					{""id"": 1, ""conditionType"":""enabled"" },
					{""id"": 2, ""conditionType"":""disabled"" },
				],
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<AllCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.Children[0].ShouldBeOfType<EnabledCondition>(),
				() => condition.Children[1].ShouldBeOfType<DisabledCondition>()
			);
		}
	}
}
