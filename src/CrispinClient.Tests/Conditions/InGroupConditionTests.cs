using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class InGroupConditionTests
	{
		private readonly InGroupCondition _condition;
		private readonly IToggleContext _query;

		public InGroupConditionTests()
		{
			_condition = new InGroupCondition
			{
				GroupName = "group",
				SearchKey = "term"
			};
			_query = Substitute.For<IToggleContext>();
		}

		[Fact]
		public void When_in_the_group()
		{
			_query.GroupContains("group", "term").Returns(true);
			_condition.IsMatch(_query).ShouldBe(true);
		}

		[Fact]
		public void When_not_in_the_group()
		{
			_query.GroupContains("group", "term").Returns(false);
			_condition.IsMatch(_query).ShouldBe(false);
		}
	}
}
