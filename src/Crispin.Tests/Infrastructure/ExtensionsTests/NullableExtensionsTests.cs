using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.ExtensionsTests
{
	public class NullableExtensionsTests
	{
		[Fact]
		public void When_a_nullable_has_a_value()
		{
			var nullable = (int?)13;
			var hasCalled = 0;
			var noneCalled = false;

			nullable.Match(
				hasValue: val => hasCalled = val,
				noValue: () => noneCalled = true);

			hasCalled.ShouldBe(13);
			noneCalled.ShouldBeFalse();
		}

		[Fact]
		public void When_a_nullable_is_null()
		{
			var nullable = (int?)null;
			var hasCalled = 0;
			var noneCalled = false;

			nullable.Match(
				hasValue: val => hasCalled = val,
				noValue: () => noneCalled = true);

			hasCalled.ShouldBe(0);
			noneCalled.ShouldBeTrue();
		}
	}
}
