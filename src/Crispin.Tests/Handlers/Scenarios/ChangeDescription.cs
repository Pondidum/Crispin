using System.Threading.Tasks;
using Crispin.Handlers.ChangeDescription;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class ChangeDescription : HandlerTest<ChangeToggleDescriptionRequest, ChangeToggleDescriptionResponse>
	{
		protected override async Task<ChangeToggleDescriptionRequest> When()
		{
			await CreateToggle();

			return new ChangeToggleDescriptionRequest(Editor, Locator, "some new description");
		}

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_contains_the_new_description() => Response.Description.ShouldBe("some new description");

		[Fact]
		public async Task The_toggle_gets_saved() => (await Read(ToggleID)).Description.ShouldBe("some new description");
	}
}
