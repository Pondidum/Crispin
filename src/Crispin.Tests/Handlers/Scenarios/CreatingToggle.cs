using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class CreatingToggle : HandlerPipelineTest<CreateToggleRequest, CreateTogglesResponse>
	{
		protected override Task<CreateToggleRequest> When()
		{
			return Task.FromResult(new CreateToggleRequest(Editor, "name-one", ""));
		}

		[Fact]
		public void The_response_contains_the_created_toggle() => Response.Toggle.ID.ShouldNotBe(ToggleID.Empty);

		[Fact]
		public async Task The_toggle_should_be_saved() => (await Read(Response.Toggle.ID)).ShouldNotBeNull();
	}
}
