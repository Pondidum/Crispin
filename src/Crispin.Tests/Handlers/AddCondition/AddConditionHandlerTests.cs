using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Conditions.ConditionTypes;
using Crispin.Events;
using Crispin.Handlers.AddCondition;
using Crispin.Infrastructure.Storage;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.AddCondition
{
	public class AddConditionHandlerTests : HandlerTest<AddConditionHandler>
	{
		private readonly Dictionary<string, object> _properties;

		public AddConditionHandlerTests()
		{
			_properties = new Dictionary<string, object>
			{
				{ ConditionBuilder.TypeKey, "enabled" }
			};
		}

		protected override AddConditionHandler CreateHandler(IStorage storage)
		{
			return new AddConditionHandler(storage);
		}

		private async Task<AddToggleConditionResponse> HandleMessage()
		{
			return await Handler.Handle(new AddToggleConditionRequest(Editor, Locator, _properties));
		}

		[Fact]
		public async Task The_updated_conditions_collection_is_returned()
		{
			var result = await HandleMessage();

			result.Condition.ShouldBeOfType<EnabledCondition>();
		}

		[Fact]
		public async Task The_toggle_is_saved_into_the_session()
		{
			await HandleMessage();

			Event<ConditionAdded>(e => e.ShouldSatisfyAllConditions(
				() => e.Properties.ShouldBe(_properties),
				() => e.Editor.ShouldBe(Editor)
			));
		}
	}
}
