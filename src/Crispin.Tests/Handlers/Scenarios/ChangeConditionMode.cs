using System.Threading.Tasks;
using Crispin.Handlers.ChangeConditionMode;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class ChangeConditionMode : HandlerTest<ChangeConditionModeRequest, ChangeConditionModeResponse>
	{
		protected override async Task<ChangeConditionModeRequest> When()
		{
			await CreateToggle();
			return new ChangeConditionModeRequest(Editor, Locator, ConditionModes.Any);
		}

		[Fact]
		public void The_response_contains_the_new_condition_mode() => Response.ConditionMode.ShouldBe(ConditionModes.Any);

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);
	}
}
