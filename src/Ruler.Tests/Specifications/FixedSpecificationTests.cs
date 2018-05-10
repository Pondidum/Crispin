using Ruler.Specifications;
using Shouldly;
using Xunit;

namespace Ruler.Tests.Specifications
{
	public class FixedSpecificationTests
	{
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void The_specification_returns_the_expected_result(bool value)
		{
			var spec = new FixedSpecification<string>(value);

			spec.IsMatch("wat").ShouldBe(value);
		}
	}
}
