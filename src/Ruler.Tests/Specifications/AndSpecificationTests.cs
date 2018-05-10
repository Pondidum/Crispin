using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Ruler.Specifications;
using Shouldly;
using Xunit;

namespace Ruler.Tests.Specifications
{
	public class AndSpecificationTests
	{
		private readonly ISpecification<string> _left;
		private readonly ISpecification<string> _right;
		private readonly AndSpecification<string> _sut;

		public AndSpecificationTests()
		{
			_left = Substitute.For<ISpecification<string>>();
			_right = Substitute.For<ISpecification<string>>();
			_sut = new AndSpecification<string>(_left, _right);
		}

		[Theory]
		[InlineData(false, false, false)]
		[InlineData(false, true, false)]
		[InlineData(true, false, false)]
		[InlineData(true, true, true)]
		public void The_right_values_are_returned(bool leftResult, bool rightResult, bool expectedResult)
		{
			_left.IsMatch(Arg.Any<string>()).Returns(leftResult);
			_right.IsMatch(Arg.Any<string>()).Returns(rightResult);

			_sut.IsMatch("wat").ShouldBe(expectedResult);
		}

		[Fact]
		public void When_left_is_false_right_is_not_evaluated()
		{
			_left.IsMatch(Arg.Any<string>()).Returns(false);
			_right.IsMatch(Arg.Any<string>()).Throws(new ShouldNotBeThrownException());

			_sut.IsMatch("wat").ShouldBe(false);

			_right.DidNotReceive().IsMatch(Arg.Any<string>());
		}
	}
}
