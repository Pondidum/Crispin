using System;
using System.Linq;
using Crispin.Rules;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Rules
{
	public class ConditionBuilderTests
	{
		private readonly ConditionBuilder _builder;

		public ConditionBuilderTests()
		{
			_builder = new ConditionBuilder(0);
		}


		[Fact]
		public void A_root_condition_cannot_be_added_if_one_exists()
		{
			_builder.AddCondition(new DisabledCondition());

			Should.Throw<NotSupportedException>(
				() => _builder.AddCondition(new EnabledCondition())
			);
		}

		[Fact]
		public void A_root_condition_can_be_added()
		{
			_builder.AddCondition(new EnabledCondition());

			_builder
				.Condition.ShouldBeOfType<EnabledCondition>()
				.ID.ShouldBe(0);
		}

		[Fact]
		public void The_id_increments_when_conditions_are_added()
		{
			_builder.AddCondition(new EnabledCondition());
			_builder.Condition.ID.ShouldBe(0);

			_builder.RemoveCondition(0);
			_builder.AddCondition(new DisabledCondition());
			_builder.Condition.ID.ShouldBe(1);
		}

		[Fact]
		public void A_condition_can_be_added_to_a_condition_supporting_children()
		{
			_builder.AddCondition(new AnyCondition());
			_builder.AddCondition(new EnabledCondition(), _builder.Condition.ID);

			_builder
				.Condition.ShouldBeOfType<AnyCondition>()
				.Children.ShouldHaveSingleItem().ShouldBeOfType<EnabledCondition>();
		}

		[Fact]
		public void Multiple_conditions_can_be_added_to_a_condition_supporting_children()
		{
			_builder.AddCondition(new AnyCondition());
			_builder.AddCondition(new EnabledCondition(), _builder.Condition.ID);
			_builder.AddCondition(new DisabledCondition(), _builder.Condition.ID);

			var root = _builder
				.Condition
				.ShouldBeOfType<AnyCondition>();

			root.Children.Select(c => c.GetType()).ShouldBe(new[]
			{
				typeof(EnabledCondition),
				typeof(DisabledCondition)
			});
		}
	}
}
