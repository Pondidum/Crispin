using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions.ConditionTypes;
using Crispin.Handlers.AddCondition;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class AddConditionAsChildCondition : HandlerTest<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		protected override async Task<AddToggleConditionRequest> When()
		{
			await CreateToggle(t => t.AddCondition(Editor, new Dictionary<string, object>
			{
				{ "type", "all" }
			}));

			var toggle = await Read(ToggleID);
			var parentCondition = toggle.Conditions.Single().ID;

			return new AddToggleConditionRequest(Editor, Locator, parentCondition, new Dictionary<string, object>
			{
				{ "type", "enabled" }
			});
		}

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_contains_the_condition() => Response.Condition.ShouldBeOfType<EnabledCondition>();

		[Fact]
		public async Task The_toggles_has_the_condition_added() => (await Read(ToggleID))
			.Conditions.ShouldHaveSingleItem()
			.ShouldBeOfType<AllCondition>()
			.Children.ShouldHaveSingleItem()
			.ShouldBeOfType<EnabledCondition>();
	}
}
