using System.Collections.Generic;
using System.Linq;
using Crispin.Conditions;
using Crispin.Conditions.ConditionTypes;
using Crispin.Events;
using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleConditionTests : ToggleTest
	{
		private static Dictionary<string, object> ConditionProperties(string type) => new Dictionary<string, object>
		{
			{ ConditionBuilder.TypeKey, type }
		};

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
			CreateToggle(new EnabledOnAnyCondition(Editor).AsAct());

			Toggle.EnableOnAllConditions(Editor);

			Toggle.ConditionMode.ShouldBe(ConditionModes.All);
			SingleEvent<EnabledOnAllConditions>().Editor.ShouldBe(Editor);
		}

		[Fact]
		public void When_switching_from_all_to_all()
		{
			CreateToggle(new EnabledOnAnyCondition(Editor).AsAct());

			Toggle.EnableOnAllConditions(Editor);
			Toggle.EnableOnAllConditions(Editor);

			SingleEvent<EnabledOnAllConditions>().Editor.ShouldBe(Editor);
		}

		[Fact]
		public void When_switching_from_any_to_any()
		{
			CreateToggle(new EnabledOnAllConditions(Editor).AsAct());

			Toggle.EnableOnAnyCondition(Editor);
			Toggle.EnableOnAnyCondition(Editor);

			SingleEvent<EnabledOnAnyCondition>().Editor.ShouldBe(Editor);
		}

		[Fact]
		public void Adding_a_condition_generates_a_well_formed_event()
		{
			var condition = new Dictionary<string, object>
			{
				{ ConditionBuilder.TypeKey, "InGroup" },
				{ "GroupName", "testGroup" },
				{ "SearchKey", "searchValue" }
			};

			CreateToggle();
			Toggle.AddCondition(Editor, condition);

			SingleEvent<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.Properties.ShouldBe(condition),
				() => e.Editor.ShouldBe(Editor),
				() => e.ConditionID.ShouldBe(ConditionID.Parse(0)),
				() => e.ParentConditionID.ShouldBeNull()
			));
		}

		[Fact]
		public void Multiple_conditions_can_be_added()
		{
			var conditionOne = ConditionProperties("Enabled");
			var conditionTwo = ConditionProperties("Not");

			CreateToggle();
			Toggle.AddCondition(Editor, conditionOne);
			Toggle.AddCondition(Editor, conditionTwo);

			Events.Length.ShouldBe(2);
			Event<ConditionAdded>(0).Data.ConditionID.ShouldBe(ConditionID.Parse(0));
			Event<ConditionAdded>(1).Data.ConditionID.ShouldBe(ConditionID.Parse(1));
		}

		[Fact]
		public void Conditions_maintain_order()
		{
			var ids = Enumerable.Range(0, 15).Select(ConditionID.Parse).ToArray();

			CreateToggle();

			foreach (var id in ids)
				Toggle.AddCondition(Editor, ConditionProperties("enabled"));

			Toggle.Conditions.Select(c => c.ID).ShouldBe(ids);
		}

		[Fact]
		public void Conditions_can_be_removed()
		{
			var one = ConditionProperties("Enabled");
			var two = ConditionProperties("Enabled");
			var three = ConditionProperties("Enabled");

			CreateToggle(
				new ConditionAdded(Editor, ConditionID.Parse(0), null, one).AsAct(),
				new ConditionAdded(Editor, ConditionID.Parse(1), null, two).AsAct(),
				new ConditionAdded(Editor, ConditionID.Parse(2), null, three).AsAct()
			);

			Toggle.RemoveCondition(Editor, ConditionID.Parse(1));

			SingleEvent<ConditionRemoved>(e => e.ShouldSatisfyAllConditions(
				() => e.ConditionID.ShouldBe(ConditionID.Parse(1)),
				() => e.Editor.ShouldBe(Editor)
			));

			Toggle.Conditions.Select(c => c.ID).ShouldBe(new[] { ConditionID.Parse(0), ConditionID.Parse(2) });
		}

		[Fact]
		public void Condition_ids_always_increment_when_removals_happen()
		{
			CreateToggle();

			var additions = 12;

			for (int i = 0; i < additions; i++)
				Toggle.AddCondition(Editor, ConditionProperties("enabled"));

			Toggle.RemoveCondition(Editor, ConditionID.Parse(5));
			Toggle.RemoveCondition(Editor, ConditionID.Parse(2));

			Toggle.AddCondition(Editor, ConditionProperties("enabled"));

			Events
				.OfType<Event<ConditionAdded>>()
				.Last()
				.Data
				.ConditionID.ShouldBe(ConditionID.Parse(additions));
		}

		[Fact]
		public void Trying_to_remove_a_non_existing_condition_throws()
		{
			CreateToggle();

			var additions = 5;
			var toRemove = additions + 3;

			for (int i = 0; i < additions; i++)
				Toggle.AddCondition(Editor, ConditionProperties("enabled"));

			Should
				.Throw<ConditionNotFoundException>(() => Toggle.RemoveCondition(Editor, ConditionID.Parse(toRemove)))
				.Message.ShouldContain(toRemove.ToString());
		}

		[Fact]
		public void Conditions_can_have_a_parent_specified()
		{
			CreateToggle(t => { t.AddCondition(Editor, ConditionProperties("any")); });

			var added = Toggle.AddCondition(Editor, ConditionProperties("enabled"), parentConditionID: ConditionID.Parse(0));

			SingleEvent<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.ConditionID.ShouldBe(added),
				() => e.Editor.ShouldBe(Editor),
				() => e.ParentConditionID.ShouldBe(ConditionID.Parse(0))
			));
		}

		[Fact]
		public void Conditions_can_be_added_to_conditions_supporting_children()
		{
			CreateToggle(t => { t.AddCondition(Editor, ConditionProperties("any")); });

			Toggle.AddCondition(Editor, ConditionProperties("enabled"), parentConditionID: ConditionID.Parse(0));

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
			CreateToggle(t => { t.AddCondition(Editor, ConditionProperties("any")); });

			Should.Throw<ConditionNotFoundException>(
				() => Toggle.AddCondition(Editor, ConditionProperties("enabled"), parentConditionID: ConditionID.Parse(13))
			);
		}

		[Fact]
		public void When_adding_a_child_to_a_condition_which_doesnt_support_children()
		{
			CreateToggle(t => { t.AddCondition(Editor, ConditionProperties("enabled")); });

			Should.Throw<ConditionException>(
				() => Toggle.AddCondition(Editor, ConditionProperties("enabled"), parentConditionID: ConditionID.Parse(0))
			);
		}

		[Fact]
		public void Child_conditions_can_be_removed()
		{
			var parentID = ConditionID.Empty;
			var childOneID = ConditionID.Empty;
			var childTwoID = ConditionID.Empty;

			CreateToggle(t =>
			{
				parentID = t.AddCondition(Editor, ConditionProperties("any"));
				childOneID = t.AddCondition(Editor, ConditionProperties("enabled"), parentID);
				childTwoID = t.AddCondition(Editor, ConditionProperties("disabled"), parentID);
			});

			Toggle.RemoveCondition(Editor, conditionID: childOneID);

			var parent = Toggle
				.Conditions
				.ShouldHaveSingleItem()
				.ShouldBeOfType<AnyCondition>();

			parent.Children.Select(c => c.ID).ShouldBe(new[] { childTwoID });
		}

		[Fact]
		public void Next_condition_id_is_correct_when_adding_a_new_condition_after_load()
		{
			CreateToggle(
				new ConditionAdded(Editor, ConditionID.Parse(0), null, ConditionProperties("enabled")).AsAct(),
				new ConditionRemoved(Editor, ConditionID.Parse(0)).AsAct(),
				new ConditionAdded(Editor, ConditionID.Parse(1), null, ConditionProperties("enabled")).AsAct()
			);

			Toggle.AddCondition(Editor, ConditionProperties("disabled"));

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
