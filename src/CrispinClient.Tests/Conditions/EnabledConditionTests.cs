using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class EnabledConditionTests
	{
		[Fact]
		public void It_always_returns_true() => new EnabledCondition()
			.IsMatch(Substitute.For<IActiveQuery>())
			.ShouldBe(true);
	}
}
