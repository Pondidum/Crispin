using System;
using Crispin.Events;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleNamingTests : ToggleTest
	{
		[Fact]
		public void When_renaming_a_toggle_the_event_is_created()
		{
			CreateToggle();
			Toggle.Rename(Editor, "new name");

			SingleEvent<ToggleRenamed>(e => e.ShouldSatisfyAllConditions(
				() => e.NewName.ShouldBe("new name"),
				() => e.Editor.ShouldBe(Editor)
			));
		}

		[Fact]
		public void When_renaming_a_toggle_the_toggle_is_updated()
		{
			CreateToggle();
			Toggle.Rename(Editor, "new name");

			Toggle.Name.ShouldBe("new name");
		}

		[Fact]
		public void When_renaming_to_the_current_name()
		{
			CreateToggle();
			Toggle.Rename(Editor, Toggle.Name);

			EventTypes.ShouldBeEmpty();
		}

		[Fact]
		public void When_renaming_the_name_cannot_be_null()
		{
			CreateToggle();

			Should.Throw<ArgumentException>(() => Toggle.Rename(Editor, null));
		}

		[Fact]
		public void When_renaming_the_name_cannot_be_whitespace()
		{
			CreateToggle();

			Should.Throw<ArgumentException>(() => Toggle.Rename(Editor, "      "));
		}

		[Fact]
		public void When_changing_the_description_of_a_toggle_the_event_is_created()
		{
			CreateToggle();
			Toggle.ChangeDescription(Editor, "new description");

			SingleEvent<ToggleDescriptionChanged>(e => e.ShouldSatisfyAllConditions(
				() => e.NewDescription.ShouldBe("new description"),
				() => e.Editor.ShouldBe(Editor)
			));
		}

		[Fact]
		public void When_changing_the_description_of_a_toggle_the_toggle_is_updated()
		{
			CreateToggle();
			Toggle.ChangeDescription(Editor, "new description");

			Toggle.Description.ShouldBe("new description");
		}

		[Fact]
		public void When_changing_the_description_to_the_current_description()
		{
			CreateToggle();
			Toggle.ChangeDescription(Editor, Toggle.Description);

			EventTypes.ShouldBeEmpty();
		}
	}
}
