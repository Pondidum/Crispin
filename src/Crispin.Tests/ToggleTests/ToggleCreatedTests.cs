using System;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleCreatedTests
	{
		[Fact]
		public void When_creating_feature_toggle_without_a_description()
		{
			var toggle = Toggle.CreateNew(name: "first-toggle");

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldNotBe(Guid.Empty),
				() => toggle.Name.ShouldBe("first-toggle"),
				() => toggle.Description.ShouldBe(string.Empty)
			);
		}

		[Fact]
		public void When_create_a_feature_toggle_with_a_description()
		{
			var toggle = Toggle.CreateNew(name: "first-toggle", description: "my cool description");

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldNotBe(Guid.Empty),
				() => toggle.Name.ShouldBe("first-toggle"),
				() => toggle.Description.ShouldBe("my cool description")
			);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("         ")]
		[InlineData("		")]
		public void When_creating_a_toggle_with_no_name(string name)
		{
			Should
				.Throw<ArgumentNullException>(() => Toggle.CreateNew(name: name))
				.Message.ShouldContain("name");
		}

		[Theory]
		[InlineData("      one")]
		[InlineData("two      ")]
		[InlineData("  three  ")]
		public void When_creating_a_toggle_and_the_name_has_leading_or_trailing_whitespace(string name)
		{
			Toggle
				.CreateNew(name: name)
				.Name
				.ShouldBe(name.Trim());
		}
	}
}