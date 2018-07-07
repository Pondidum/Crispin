using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Events;
using Crispin.Handlers.RemoveCondition;
using Crispin.Infrastructure.Storage;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class RemoveConditionHandlerTests : HandlerTest<RemoveConditionHandler>
	{
		protected override RemoveConditionHandler CreateHandler(IStorageSession session) => new RemoveConditionHandler(session);

		protected override void InitialiseToggle(Toggle toggle)
		{
			toggle.AddCondition(Editor, new Dictionary<string, object>
			{
				{ ConditionBuilder.TypeKey, "enabled" }
			});
		}

		[Fact]
		public async Task The_updated_conditions_collection_is_returned()
		{
			var conditionID = Toggle.Conditions.Single().ID;
			var result = await Handler.Handle(new RemoveToggleConditionRequest(Editor, Locator, conditionID));
			await Session.Commit();

			result.Conditions.ShouldBeEmpty();
		}

		[Fact]
		public async Task The_updated_condition_mode_is_returned()
		{
			var conditionID = Toggle.Conditions.Single().ID;
			var result = await Handler.Handle(new RemoveToggleConditionRequest(Editor, Locator, conditionID));
			await Session.Commit();

			result.ConditionMode.ShouldBe(Toggle.ConditionMode);
		}

		[Fact]
		public async Task The_toggle_is_saved_into_the_session()
		{
			var conditionID = Toggle.Conditions.Single().ID;
			await Handler.Handle(new RemoveToggleConditionRequest(Editor, Locator, conditionID));
			await Session.Commit();

			Event<ConditionRemoved>(e => e.ShouldSatisfyAllConditions(
				() => e.ConditionID.ShouldBe(conditionID),
				() => e.Editor.ShouldBe(Editor)
			));
		}
	}
}
