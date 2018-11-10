using Crispin.Events;
using Crispin.Views;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Views
{
	public class ToggleViewTests
	{
		private readonly ToggleView _view;
		private readonly EditorID _editor;
		private readonly ToggleID _id;

		public ToggleViewTests()
		{
			_view = new ToggleView();
			_editor = EditorID.Parse("editor");
			_id = ToggleID.CreateNew();
		}

		[Fact]
		public void When_applying_toggle_created()
		{
			_view.Apply(new ToggleCreated(_editor, _id, "a toggle", "some description"));

			_view.ShouldSatisfyAllConditions(
				() => _view.ID.ShouldBe(_id),
				() => _view.Name.ShouldBe("a toggle"),
				() => _view.Description.ShouldBe("some description")
			);
		}

		[Fact]
		public void When_applying_toggle_renamed()
		{
			_view.Apply(new ToggleRenamed(_editor, "new name!"));

			_view.Name.ShouldBe("new name!");
		}

		[Fact]
		public void When_applying_tag_added()
		{
			_view.Apply(new TagAdded(_editor, "testing"));

			_view.Tags.ShouldBe(new[] { "testing" });
		}

		[Fact]
		public void When_applying_tag_removed()
		{
			_view.Tags.Add("to go");
			_view.Apply(new TagRemoved(_editor, "to go"));

			_view.Tags.ShouldBeEmpty();
		}

		[Fact]
		public void When_applying_condition_mode_all()
		{
			_view.ConditionMode = ConditionModes.Any;
			_view.Apply(new EnabledOnAllConditions(_editor));

			_view.ConditionMode.ShouldBe(ConditionModes.All);
		}

		[Fact]
		public void When_applying_condition_mode_any()
		{
			_view.ConditionMode = ConditionModes.All;
			_view.Apply(new EnabledOnAnyCondition(_editor));

			_view.ConditionMode.ShouldBe(ConditionModes.Any);
		}
	}
}
