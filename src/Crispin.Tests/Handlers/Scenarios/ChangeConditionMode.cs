using System.Threading.Tasks;
using Crispin.Handlers.ChangeConditionMode;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class ChangeConditionMode : HandlerPipelineTest<ChangeConditionModeRequest, ChangeConditionModeResponse>
	{
		public override async Task InitializeAsync()
		{
			await CreateToggle();
			await Send(new ChangeConditionModeRequest(Editor, ToggleLocator.Create(ToggleID), ConditionModes.Any));
		}

		[Fact]
		public void The_response_contains_the_new_condition_mode() => Response.ConditionMode.ShouldBe(ConditionModes.Any);

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);
	}
}
