using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Crispin.Conditions;
using Crispin.Events;
using Crispin.Infrastructure;
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
				() => e.Condition.ID.ShouldBe(ConditionID.Parse(0)),
				() => e.ParentConditionID.ShouldBeNull()
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
			Event<ConditionAdded>(0).Condition.ID.ShouldBe(ConditionID.Parse(0));
			Event<ConditionAdded>(1).Condition.ID.ShouldBe(ConditionID.Parse(1));
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
			var one = new EnabledCondition { ID = ConditionID.Parse(0) };
			var two = new EnabledCondition { ID = ConditionID.Parse(1) };
			var three = new EnabledCondition { ID = ConditionID.Parse(2) };

			CreateToggle(
				new ConditionAdded(Editor, one),
				new ConditionAdded(Editor, two),
				new ConditionAdded(Editor, three)
			);

			Toggle.RemoveCondition(Editor, ConditionID.Parse(1));

			SingleEvent<ConditionRemoved>(e => e.ShouldSatisfyAllConditions(
				() => e.ConditionID.ShouldBe(ConditionID.Parse(1)),
				() => e.Editor.ShouldBe(Editor)
			));

			Toggle.Conditions.ShouldBe(new[] { one, three });
		}

		[Fact]
		public void Condition_ids_always_increment_when_removals_happen()
		{
			CreateToggle();

			var additions = 12;

			for (int i = 0; i < additions; i++)
				Toggle.AddCondition(Editor, new EnabledCondition());

			Toggle.RemoveCondition(Editor, ConditionID.Parse(5));
			Toggle.RemoveCondition(Editor, ConditionID.Parse(2));

			Toggle.AddCondition(Editor, new EnabledCondition());

			Events
				.OfType<ConditionAdded>()
				.Last()
				.Condition.ID.ShouldBe(ConditionID.Parse(additions));
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
				.Throw<ConditionNotFoundException>(() => Toggle.RemoveCondition(Editor, ConditionID.Parse(toRemove)))
				.Message.ShouldContain(toRemove.ToString());
		}

		[Fact]
		public void Conditions_can_have_a_parent_specified()
		{
			CreateToggle(t => { t.AddCondition(Editor, new AnyCondition()); });

			Toggle.AddCondition(Editor, new EnabledCondition(), parentConditionID: ConditionID.Parse(0));

			SingleEvent<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.Condition.ShouldBeOfType<EnabledCondition>(),
				() => e.Editor.ShouldBe(Editor),
				() => e.ParentConditionID.ShouldBe(ConditionID.Parse(0))
			));
		}

		[Fact]
		public void Conditions_can_be_added_to_conditions_supporting_children()
		{
			CreateToggle(t => { t.AddCondition(Editor, new AnyCondition()); });

			Toggle.AddCondition(Editor, new EnabledCondition(), parentConditionID: ConditionID.Parse(0));

			var parent = Toggle
				.Conditions
				.ShouldHaveSingleItem()
				.ShouldBeOfType<AnyCondition>();

			parent
				.Children
				.ShouldHaveSingleItem()
				.ShouldBeOfType<EnabledCondition>();
		}

		[Fact]
		public void When_the_parent_condition_doesnt_exist()
		{
			CreateToggle(t => { t.AddCondition(Editor, new AnyCondition()); });

			Should.Throw<ConditionNotFoundException>(
				() => Toggle.AddCondition(Editor, new EnabledCondition(), parentConditionID: ConditionID.Parse(13))
			);
		}

		[Fact]
		public void When_adding_a_child_to_a_condition_which_doesnt_support_children()
		{
			CreateToggle(t => { t.AddCondition(Editor, new EnabledCondition()); });

			Should.Throw<ConditionException>(
				() => Toggle.AddCondition(Editor, new EnabledCondition(), parentConditionID: ConditionID.Parse(0))
			);
		}

		[Fact]
		public void Child_conditions_can_be_removed()
		{
			var parent = new AnyCondition();
			var childOne = new EnabledCondition();
			var childTwo = new DisabledCondition();

			CreateToggle(t =>
			{
				t.AddCondition(Editor, parent);
				t.AddCondition(Editor, childOne, parent.ID);
				t.AddCondition(Editor, childTwo, parent.ID);
			});

			Toggle.RemoveCondition(Editor, conditionID: childOne.ID);

			Toggle
				.Conditions
				.ShouldHaveSingleItem()
				.ShouldBeOfType<AnyCondition>();

			parent.Children.ShouldBe(new[] { childTwo });
		}

		[Fact]
		public void Next_condition_id_is_correct_when_adding_a_new_condition_after_load()
		{
			CreateToggle(
				new ConditionAdded(Editor, new EnabledCondition { ID = ConditionID.Parse(0) }),
				new ConditionRemoved(Editor, ConditionID.Parse(0)),
				new ConditionAdded(Editor, new EnabledCondition { ID = ConditionID.Parse(1) })
			);

			Toggle.AddCondition(Editor, new DisabledCondition());

			var expected = ConditionID.Parse(2);
			Toggle
				.Conditions
				.OfType<DisabledCondition>()
				.Last()
				.ID
				.ShouldBe(expected);
		}
	}
}
