using System.Linq;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Rules;
using Newtonsoft.Json;
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
				() => e.Condition.ID.ShouldBe(0)
			));
		}

		[Fact]
		public void Multiple_conditions_can_be_added()
		{
			var conditionOne = new EnabledCondition();
			var conditionTwo = new NotCondition();

			CreateToggle();
			Toggle.AddCondition(Editor, conditionOne);
			Toggle.AddCondition(Editor, conditionTwo);

			Events.Length.ShouldBe(2);
			Event<ConditionAdded>(0).Condition.ID.ShouldBe(0);
			Event<ConditionAdded>(1).Condition.ID.ShouldBe(1);
		}

		[Fact]
		public void Conditions_maintain_order()
		{
			var conditions = Enumerable.Range(0, 15).Select(i => new EnabledCondition()).ToArray();

			CreateToggle();

			foreach (var condition in conditions)
				Toggle.AddCondition(Editor, condition);

			Toggle.Conditions.ShouldBe(conditions);
		}

		[Fact]
		public void Conditions_can_be_removed()
		{
			var one = new EnabledCondition { ID = 0 };
			var two = new EnabledCondition { ID = 1 };
			var three = new EnabledCondition { ID = 2 };

			CreateToggle(
				new ConditionAdded(Editor, one),
				new ConditionAdded(Editor, two),
				new ConditionAdded(Editor, three)
			);

			Toggle.RemoveCondition(Editor, 1);

			SingleEvent<ConditionRemoved>(e => e.ShouldSatisfyAllConditions(
				() => e.ConditionID.ShouldBe(1),
				() => e.Editor.ShouldBe(Editor))
			);

			Toggle.Conditions.ShouldBe(new[] { one, three });
		}

		[Fact]
		public void Condition_ids_always_increment_when_removals_happen()
		{
			CreateToggle();

			var additions = 12;

			for (int i = 0; i < additions; i++)
				Toggle.AddCondition(Editor, new EnabledCondition());

			Toggle.RemoveCondition(Editor, 5);
			Toggle.RemoveCondition(Editor, 2);

			Toggle.AddCondition(Editor, new EnabledCondition());

			Events
				.OfType<ConditionAdded>()
				.Last()
				.Condition.ID.ShouldBe(additions);
		}

		[Fact]
		public void Trying_to_remove_a_non_existing_condition_throws()
		{
			CreateToggle();

			var additions = 5;
			var toRemove = additions + 3;

			for (int i = 0; i < additions; i++)
				Toggle.AddCondition(Editor, new EnabledCondition());

			Should
				.Throw<ConditionNotFoundException>(() => Toggle.RemoveCondition(Editor, toRemove))
				.Message.ShouldContain(toRemove.ToString());
		}
	}
}
