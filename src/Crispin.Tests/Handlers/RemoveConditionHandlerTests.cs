using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions.ConditionTypes;
using Crispin.Events;
using Crispin.Handlers.RemoveCondition;
using Crispin.Infrastructure.Storage;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class RemoveConditionHandlerTests : HandlerTest<RemoveConditionHandler>
	{
		protected override RemoveConditionHandler CreateHandler(IStorage storage) => new RemoveConditionHandler(storage);

		protected override void InitialiseToggle(Toggle toggle)
		{
			toggle.AddCondition(Editor, new EnabledCondition());
		}

		[Fact]
		public async Task The_updated_conditions_collection_is_returned()
		{
			var conditionID = Toggle.Conditions.Single().ID;
			var result = await Handler.Handle(new RemoveToggleConditionRequest(Editor, Locator, conditionID));

			result.Conditions.ShouldBeEmpty();
		}

		[Fact]
		public async Task The_updated_condition_mode_is_returned()
		{
			var conditionID = Toggle.Conditions.Single().ID;
			var result = await Handler.Handle(new RemoveToggleConditionRequest(Editor, Locator, conditionID));

			result.ConditionMode.ShouldBe(Toggle.ConditionMode);
		}

		[Fact]
		public async Task The_toggle_is_saved_into_the_session()
		{
			var conditionID = Toggle.Conditions.Single().ID;
			await Handler.Handle(new RemoveToggleConditionRequest(Editor, Locator, conditionID));

			Event<ConditionRemoved>(e => e.ShouldSatisfyAllConditions(
				() => e.ConditionID.ShouldBe(conditionID),
				() => e.Editor.ShouldBe(Editor)
			));
		}
	}
}
