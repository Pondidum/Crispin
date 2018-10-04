using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class InGroupConditionTests : ConditionTests<InGroupCondition>
	{
		private readonly IToggleContext _query;

		public InGroupConditionTests()
		{
			_query = Substitute.For<IToggleContext>();

			Sut.GroupName = "group";
			Sut.SearchKey = "term";
		}

		[Fact]
		public void When_in_the_group()
		{
			_query.GroupContains("group", "term").Returns(true);
			Sut.IsMatch(Stats, _query).ShouldBe(true);
		}

		[Fact]
		public void When_not_in_the_group()
		{
			_query.GroupContains("group", "term").Returns(false);
			Sut.IsMatch(Stats, _query).ShouldBe(false);
		}

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""ingroup"",
				""groupName"":""where to look"",
				""searchKey"":""what to find""
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<InGroupCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.GroupName.ShouldBe("where to look"),
				() => condition.SearchKey.ShouldBe("what to find"),
				() => condition.Children.ShouldBeEmpty()
			);
		}
	}
}
