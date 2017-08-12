using System;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleCreatedTests : ToggleTest
	{
		[Fact]
		public void When_creating_feature_toggle_without_a_description()
		{
			Toggle = Toggle.CreateNew(
				getCurrentUserID: () => string.Empty,
				name: "first-toggle");

			Toggle.ShouldSatisfyAllConditions(
				() => Toggle.ID.ShouldNotBe(Guid.Empty),
				() => Toggle.Name.ShouldBe("first-toggle"),
				() => Toggle.Description.ShouldBe(string.Empty)
			);
		}

		[Fact]
		public void When_create_a_feature_toggle_with_a_description()
		{
			Toggle = Toggle.CreateNew(
				getCurrentUserID: () => string.Empty,
				name: "first-toggle",
				description: "my cool description");

			Toggle.ShouldSatisfyAllConditions(
				() => Toggle.ID.ShouldNotBe(Guid.Empty),
				() => Toggle.Name.ShouldBe("first-toggle"),
				() => Toggle.Description.ShouldBe("my cool description")
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
				.Throw<ArgumentNullException>(() =>
				{
					Toggle.CreateNew(getCurrentUserID: () => string.Empty, name: name);
				})
				.Message.ShouldContain("name");
		}

		[Theory]
		[InlineData("      one")]
		[InlineData("two      ")]
		[InlineData("  three  ")]
		public void When_creating_a_toggle_and_the_name_has_leading_or_trailing_whitespace(string name)
		{
			Toggle
				.CreateNew(getCurrentUserID: () => string.Empty, name: name)
				.Name
				.ShouldBe(name.Trim());
		}
	}
}