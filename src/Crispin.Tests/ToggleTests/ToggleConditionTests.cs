using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Rules;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleConditionTests : ToggleTest
	{
		[Fact]
		public void A_toggle_starts_in_all_conditions_mode()
		{
			CreateToggle();

			Toggle.ConditionMode.ShouldBe(ConditionModes.All);
		}

		[Fact]
		public void When_switching_to_any_condition()
		{
			CreateToggle();

			Toggle.EnableOnAnyCondition(Editor);

			Toggle.ConditionMode.ShouldBe(ConditionModes.Any);
			SingleEvent<EnabledOnAnyCondition>().Editor.ShouldBe(Editor);
		}

		[Fact]
		public void When_switching_to_all_conditions()
		{
			CreateToggle(new EnabledOnAnyCondition(Editor));

			Toggle.EnableOnAllConditions(Editor);

			Toggle.ConditionMode.ShouldBe(ConditionModes.All);
			SingleEvent<EnabledOnAllConditions>().Editor.ShouldBe(Editor);
		}

		[Fact]
		public void When_switching_from_all_to_all()
		{
			CreateToggle(new EnabledOnAnyCondition(Editor));

			Toggle.EnableOnAllConditions(Editor);
			Toggle.EnableOnAllConditions(Editor);

			SingleEvent<EnabledOnAllConditions>().Editor.ShouldBe(Editor);
		}

		[Fact]
		public void When_switching_from_any_to_any()
		{
			CreateToggle(new EnabledOnAllConditions(Editor));

			Toggle.EnableOnAnyCondition(Editor);
			Toggle.EnableOnAnyCondition(Editor);

			SingleEvent<EnabledOnAnyCondition>().Editor.ShouldBe(Editor);
		}

		[Fact]
		public void Adding_a_condition_generates_a_well_formed_event()
		{
			var condition = new InGroupCondition
			{
				GroupName = "testGroup",
				SearchKey = "searchValue"
			};

			CreateToggle();
			Toggle.AddCondition(Editor, condition);

			SingleEvent<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.Condition.ShouldBe(condition),
				() => e.Editor.ShouldBe(Editor),
				() => e.ConditionID.ShouldBe(0)
			));
		}
	}
}
