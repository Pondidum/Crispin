using System.Linq;
using System.Threading.Tasks;
using Crispin.Handlers.GetAll;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class GetMultipleToggles : HandlerTest<GetAllTogglesRequest, GetAllTogglesResponse>
	{
		protected override async Task<GetAllTogglesRequest> When()
		{
			await CreateToggle();
			await CreateToggle();
			await CreateToggle();

			return new GetAllTogglesRequest();
		}

		[Fact]
		public void The_response_contains_all_the_toggles() => Response.Toggles.Count().ShouldBe(3);
	}
}
