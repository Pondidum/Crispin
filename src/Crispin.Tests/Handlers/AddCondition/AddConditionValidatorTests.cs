using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Handlers.AddCondition;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.AddCondition
{
	public class AddConditionValidatorTests
	{
		private readonly IConditionBuilder _builder;
		private readonly AddConditionValidator _validator;
		private readonly Dictionary<string, object> _props;

		public AddConditionValidatorTests()
		{
			_builder = Substitute.For<IConditionBuilder>();
			_validator = new AddConditionValidator(_builder);
			_props = new Dictionary<string, object>();
		}

		private Task<ICollection<string>> ValidateMessage() => _validator.Validate(
			new AddToggleConditionRequest(
				EditorID.Parse("test"),
				ToggleLocator.Create("wat"),
				_props
			));

		[Fact]
		public async Task When_the_builder_has_errors()
		{
			_builder.CanCreateFrom(_props).Returns(new[] { "builder validation error" });
			_builder.CreateCondition(_props).Returns(new InvalidCondition());

			var results = await ValidateMessage();

			results.ShouldBe(new[] { "builder validation error" });
		}

		[Fact]
		public async Task When_the_condition_has_errors()
		{
			_builder.CanCreateFrom(_props).Returns(Enumerable.Empty<string>());
			_builder.CreateCondition(_props).Returns(new InvalidCondition());

			var results = await ValidateMessage();

			results.ShouldBe(new[] { "condition validation error" });
		}

		[Fact]
		public async Task When_the_both_are_fine()
		{
			_builder.CanCreateFrom(_props).Returns(Enumerable.Empty<string>());
			_builder.CreateCondition(_props).Returns(new ValidCondition());

			var results = await ValidateMessage();

			results.ShouldBeEmpty();
		}

		private class ValidCondition : Condition
		{
		}

		private class InvalidCondition : Condition
		{
			public override IEnumerable<string> Validate()
			{
				yield return "condition validation error";
			}
		}
	}
}
