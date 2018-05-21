using System.Linq;
using Crispin.Conditions;
using Crispin.Events;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Conditions
{
	public class ConditionBuilderTests
	{
		private readonly ConditionBuilder _builder;

		public ConditionBuilderTests()
		{
			_builder = new ConditionBuilder();
		}

		[Fact]
		public void Multiple_conditions_can_be_added()
		{
			var conditionOne = new EnabledCondition();
			var conditionTwo = new NotCondition();

			_builder.Add(conditionOne);
			_builder.Add(conditionTwo);

			_builder.All.ShouldBe(new Condition[] { conditionOne, conditionTwo });
		}

		[Fact]
		public void Conditions_maintain_order()
		{
			var conditions = Enumerable.Range(0, 15).Select(i => new EnabledCondition()).ToArray();


			foreach (var condition in conditions)
				_builder.Add(condition);

			_builder.All.ShouldBe(conditions);
		}

		[Fact]
		public void Conditions_can_be_removed()
		{
			var one = new EnabledCondition { ID = 0 };
			var two = new EnabledCondition { ID = 1 };
			var three = new EnabledCondition { ID = 2 };

			_builder.Add(one);
			_builder.Add(two);
			_builder.Add(three);

			_builder.Remove(two.ID);
			_builder.All.ShouldBe(new[] { one, three });
		}

		[Fact]
		public void Trying_to_remove_a_non_existing_condition_throws()
		{
			var additions = 5;
			var toRemove = additions + 3;

			for (int i = 0; i < additions; i++)
				_builder.Add(new EnabledCondition());

			Should
				.Throw<ConditionNotFoundException>(() => _builder.Remove(toRemove))
				.Message.ShouldContain(toRemove.ToString());
		}

		[Fact]
		public void Conditions_can_be_added_to_conditions_supporting_children()
		{
			_builder.Add(new AnyCondition());
			_builder.Add(new EnabledCondition(), parentConditionID: 0);

			var parent = _builder
				.All
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
			_builder.Add(new AnyCondition());

			Should.Throw<ConditionNotFoundException>(
				() => _builder.Add(new EnabledCondition(), parentConditionID: 13)
			);
		}

		[Fact]
		public void When_adding_a_child_to_a_condition_which_doesnt_support_children()
		{
			_builder.Add(new EnabledCondition { ID = 0 });

			Should.Throw<ConditionException>(
				() => _builder.Add(new EnabledCondition(), parentConditionID: 0)
			);
		}

		[Fact]
		public void Child_conditions_can_be_removed()
		{
			var parent = new AnyCondition { ID = 0 };
			var childOne = new EnabledCondition { ID = 1 };
			var childTwo = new DisabledCondition { ID = 2 };

			_builder.Add(parent);
			_builder.Add(childOne, parent.ID);
			_builder.Add(childTwo, parent.ID);

			_builder.Remove(conditionID: childOne.ID);

			_builder
				.All
				.ShouldHaveSingleItem()
				.ShouldBeOfType<AnyCondition>();

			parent.Children.ShouldBe(new[] { childTwo });
		}
	}
}
