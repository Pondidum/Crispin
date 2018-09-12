using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class DisabledConditionTests
	{
		[Fact]
		public void It_always_returns_false() => new DisabledCondition()
			.IsMatch(Substitute.For<IActiveQuery>())
			.ShouldBe(false);
	}
}
