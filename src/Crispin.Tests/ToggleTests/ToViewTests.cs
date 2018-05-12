using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToViewTests : ToggleTest
	{
		public ToViewTests()
		{
			CreateToggle();
		}

		[Fact]
		public void When_creating_a_view()
		{
			var view = Toggle.ToView();

			view.ShouldSatisfyAllConditions(
				() => view.ID.ShouldBe(Toggle.ID),
				() => view.Name.ShouldBe(Toggle.Name),
				() => view.Description.ShouldBe(Toggle.Description)
			);
		}

		[Fact]
		public void Changing_the_view_doesnt_affect_the_toggle()
		{
			var userID = UserID.Parse("123");
			Toggle.AddTag(Editor, "wat");

			var view = Toggle.ToView();

			view.Tags.Remove("wat");

			Toggle.Tags.ShouldBe(new[] { "wat" });
		}
	}
}
