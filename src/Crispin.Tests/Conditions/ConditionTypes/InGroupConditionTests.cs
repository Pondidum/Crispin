using System.Linq;
using Crispin.Conditions.ConditionTypes;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Conditions.ConditionTypes
{
	public class InGroupConditionTests
	{
		[Fact]
		public void When_both_values_are_missing()
		{
			var condition = new InGroupCondition { GroupName = null, SearchKey = null };

			condition
				.Validate()
				.Count()
				.ShouldBe(2);
		}

		[Theory]
		[InlineData("group", null, "'SearchKey' is missing")]
		[InlineData(null, "search", "'GroupName' is missing")]
		[InlineData("group", "", "'SearchKey' is missing")]
		[InlineData("", "search", "'GroupName' is missing")]
		public void When_a_value_is_missing(string group, string search, string expectedMessage)
		{
			var condition = new InGroupCondition
			{
				GroupName = group,
				SearchKey = search
			};

			condition
				.Validate()
				.ShouldHaveSingleItem()
				.ShouldContain(expectedMessage);
		}

		[Fact]
		public void When_both_properties_are_present()
		{
			var condition = new InGroupCondition { GroupName = "group", SearchKey = "search" };

			condition
				.Validate()
				.ShouldBeEmpty();
		}
	}
}
