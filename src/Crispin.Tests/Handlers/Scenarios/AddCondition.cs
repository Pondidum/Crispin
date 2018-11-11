using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Conditions.ConditionTypes;
using Crispin.Handlers.AddCondition;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class AddCondition : HandlerPipelineTest<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		public override async Task InitializeAsync()
		{
			await CreateToggle();

			await Send(new AddToggleConditionRequest(Editor, ToggleLocator.Create(ToggleID), new Dictionary<string, object>
			{
				{ "type", "enabled" }
			}));
		}

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_contains_the_condition() => Response.Condition.ShouldBeOfType<EnabledCondition>();

		[Fact]
		public async Task The_toggles_has_the_condition_added() => (await Read(ToggleID))
			.Conditions.ShouldHaveSingleItem()
			.ShouldBeOfType<EnabledCondition>();
	}
}
