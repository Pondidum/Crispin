using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class GetSingleNonExistingToggle : HandlerTest<GetToggleRequest, GetToggleResponse>
	{
		protected override async Task<GetToggleRequest> When()
		{
			await CreateToggle();
			return new GetToggleRequest(ToggleLocator.Create(ToggleID.CreateNew()));
		}

		[Fact]
		public void The_response_toggle_is_null() => Response.Toggle.ShouldBeNull();

		[Fact]
		public void There_is_no_exception_thrown() => Exception.ShouldBeNull();
	}
}
