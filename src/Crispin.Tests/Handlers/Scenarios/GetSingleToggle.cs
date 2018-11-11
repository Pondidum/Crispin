using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class GetSingleToggle : HandlerTest<GetToggleRequest, GetToggleResponse>
	{
		protected override async Task<GetToggleRequest> When()
		{
			await CreateToggle();
			return new GetToggleRequest(Locator);
		}

		[Fact]
		public void The_response_contains_the_toggle() => Response.Toggle.ID.ShouldBe(ToggleID);
	}
}
