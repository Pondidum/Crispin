using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Conditions.ConditionTypes;
using Crispin.Events;
using Crispin.Handlers.AddCondition;
using Crispin.Infrastructure.Storage;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class AddConditionHandlerTests : HandlerTest<AddConditionHandler>
	{
		protected override AddConditionHandler CreateHandler(IStorage storage)
		{
			var builder = Substitute.For<IConditionBuilder>();
			builder
				.CreateCondition(Arg.Any<Dictionary<string, object>>())
				.Returns(new EnabledCondition());

			return new AddConditionHandler(storage, builder);
		}

		[Fact]
		public async Task The_updated_conditions_collection_is_returned()
		{
			var condition = new Dictionary<string, object>();
			var result = await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, condition));

			result.Conditions.ShouldHaveSingleItem().ShouldBeOfType<EnabledCondition>();
		}

		[Fact]
		public async Task The_updated_condition_mode_is_returned()
		{
			var condition = new Dictionary<string, object>();
			var result = await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, condition));

			result.ConditionMode.ShouldBe(Toggle.ConditionMode);
		}

		[Fact]
		public async Task The_toggle_is_saved_into_the_session()
		{
			var condition = new Dictionary<string, object>();
			await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, condition));

			Event<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.Condition.ShouldBeOfType<EnabledCondition>(),
				() => e.Editor.ShouldBe(Editor)
			));
		}
	}
}
