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
		protected override AddConditionHandler CreateHandler(IStorage storage) => new AddConditionHandler(storage);

		[Fact]
		public async Task The_updated_conditions_collection_is_returned()
		{
			var condition = new EnabledCondition();
			var result = await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, condition));

			result.Conditions.ShouldBe(new[] { condition });
		}

		[Fact]
		public async Task The_updated_condition_mode_is_returned()
		{
			var condition = new EnabledCondition();
			var result = await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, condition));

			result.ConditionMode.ShouldBe(Toggle.ConditionMode);
		}

		[Fact]
		public async Task The_toggle_is_saved_into_the_session()
		{
			var condition = new EnabledCondition();
			var result = await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, condition));

			Event<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.Condition.ShouldBe(condition),
				() => e.Editor.ShouldBe(Editor)
			));
		}
	}
}
