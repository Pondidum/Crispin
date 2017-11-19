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
		public void When_there_is_state()
		{
			Toggle.ChangeDefaultState(Editor, States.On);
			Toggle.ChangeState(Editor, UserID.Parse("123"), States.On);

			var view = Toggle.ToView();

			view.ShouldSatisfyAllConditions(
				() => view.State.Anonymous.ShouldBe(States.On),
				() => view.State.Groups.ShouldBeEmpty(),
				() => view.State.Users.ShouldContainKeyAndValue(UserID.Parse("123"), States.On)
			);
		}

		[Fact]
		public void Changing_the_view_doesnt_affect_the_toggle()
		{
			var userID = UserID.Parse("123");
			Toggle.ChangeState(Editor, userID, States.On);

			var view = Toggle.ToView();

			view.State.Users[userID] = States.Off;

			Toggle.IsActive(Membership, userID).ShouldBe(true);
		}
	}
}
