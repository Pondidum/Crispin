using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class NotConditionTests
	{
		private readonly Condition _inner;
		private readonly NotCondition _sut;

		public NotConditionTests()
		{
			_inner = Substitute.For<Condition>();
			_sut = new NotCondition { Children = new[] { _inner } };
		}

		[Fact]
		public void When_the_inner_spec_is_true()
		{
			_inner.IsMatch(Arg.Any<IToggleContext>()).Returns(true);

			_sut.IsMatch(Substitute.For<IToggleContext>()).ShouldBeFalse();
		}

		[Fact]
		public void When_the_inner_spec_is_false()
		{
			_inner.IsMatch(Arg.Any<IToggleContext>()).Returns(false);

			_sut.IsMatch(Substitute.For<IToggleContext>()).ShouldBeTrue();
		}
	}
}
