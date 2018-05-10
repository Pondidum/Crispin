using NSubstitute;
using Ruler.Specifications;
using Shouldly;
using Xunit;

namespace Ruler.Tests.Specifications
{
	public class NotSpecificationTests
	{
		private readonly ISpecification<string> _inner;
		private readonly NotSpecification<string> _sut;

		public NotSpecificationTests()
		{
			_inner = Substitute.For<ISpecification<string>>();
			_sut = new NotSpecification<string>(_inner);
		}

		[Fact]
		public void When_the_inner_spec_is_true()
		{
			_inner.IsMatch(Arg.Any<string>()).Returns(true);

			_sut.IsMatch("wat").ShouldBeFalse();
		}

		[Fact]
		public void When_the_inner_spec_is_false()
		{
			_inner.IsMatch(Arg.Any<string>()).Returns(false);

			_sut.IsMatch("wat").ShouldBeTrue();
		}
	}
}
