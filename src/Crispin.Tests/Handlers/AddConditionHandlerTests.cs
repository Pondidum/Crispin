using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Conditions.ConditionTypes;
using Crispin.Events;
using Crispin.Handlers.AddCondition;
using Crispin.Infrastructure.Storage;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class AddConditionHandlerTests : HandlerTest<AddConditionHandler>
	{
		protected override AddConditionHandler CreateHandler(IStorage storage)
		{
			return new AddConditionHandler(storage);
		}

		private async Task<AddToggleConditionResponse> HandleMessage()
		{
			return await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, new Dictionary<string, object>())
			{
				Condition = new EnabledCondition()
			});
		}

		[Fact]
		public async Task The_updated_conditions_collection_is_returned()
		{
			var result = await HandleMessage();

			result.Conditions.ShouldHaveSingleItem().ShouldBeOfType<EnabledCondition>();
		}

		[Fact]
		public async Task The_updated_condition_mode_is_returned()
		{
			var result = await HandleMessage();

			result.ConditionMode.ShouldBe(Toggle.ConditionMode);
		}

		[Fact]
		public async Task The_toggle_is_saved_into_the_session()
		{
			await HandleMessage();

			Event<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.Condition.ShouldBeOfType<EnabledCondition>(),
				() => e.Editor.ShouldBe(Editor)
			));
		}
	}
}
