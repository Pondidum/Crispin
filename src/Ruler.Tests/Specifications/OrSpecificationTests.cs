using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Ruler.Specifications;
using Shouldly;
using Xunit;

namespace Ruler.Tests.Specifications
{
	public class OrSpecificationTests
	{
		private readonly ISpecification<string> _left;
		private readonly ISpecification<string> _right;
		private readonly OrSpecification<string> _sut;

		public OrSpecificationTests()
		{
			_left = Substitute.For<ISpecification<string>>();
			_right = Substitute.For<ISpecification<string>>();
			_sut = new OrSpecification<string>(_left, _right);
		}

		[Theory]
		[InlineData(false, false, false)]
		[InlineData(false, true, true)]
		[InlineData(true, false, true)]
		[InlineData(true, true, true)]
		public void The_right_values_are_returned(bool leftResult, bool rightResult, bool expectedResult)
		{
			_left.IsMatch(Arg.Any<string>()).Returns(leftResult);
			_right.IsMatch(Arg.Any<string>()).Returns(rightResult);

			_sut.IsMatch("wat").ShouldBe(expectedResult);
		}

		[Fact]
		public void When_left_is_true_right_is_not_evaluated()
		{
			_left.IsMatch(Arg.Any<string>()).Returns(true);
			_right.IsMatch(Arg.Any<string>()).Throws(new ShouldNotBeThrownException());

			_sut.IsMatch("wat").ShouldBe(true);

			_right.DidNotReceive().IsMatch(Arg.Any<string>());
		}
	}
}
