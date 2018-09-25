using System.Linq;
using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class NotConditionTests : ConditionTests<NotCondition>
	{
		public NotConditionTests()
		{
			Sut.Children = Sut.Children.Take(1).ToArray();
		}

		[Fact]
		public void When_the_inner_spec_is_true()
		{
			ChildConditions[0].IsMatch(Reporter, Arg.Any<IToggleContext>()).Returns(true);

			Sut.IsMatch(Reporter, Substitute.For<IToggleContext>()).ShouldBeFalse();
		}

		[Fact]
		public void When_the_inner_spec_is_false()
		{
			ChildConditions[0].IsMatch(Reporter, Arg.Any<IToggleContext>()).Returns(false);

			Sut.IsMatch(Reporter, Substitute.For<IToggleContext>()).ShouldBeTrue();
		}

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""not"",
				""children"":[
					{""id"": 1, ""conditionType"":""enabled"" }
				]
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<NotCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.Children.ShouldHaveSingleItem().ShouldBeOfType<EnabledCondition>()
			);
		}
	}
}
