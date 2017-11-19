using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Projections;
using Crispin.Views;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class GetSingleToggleTests : TogglesControllerTests
	{
		public GetSingleToggleTests()
		{
			Mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse { Toggle = new ToggleView() });
		}

		[Fact]
		public async Task When_fetching_a_single_toggle_by_id()
		{
			var locator = ToggleLocator.Create(ToggleID.CreateNew());
			var response = (JsonResult)await Controller.Get(locator);

			await Mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == locator));
			response.Value.ShouldBeOfType<ToggleView>();
		}

		[Fact]
		public async Task When_fetching_a_single_toggle_by_name()
		{
			var locator = ToggleLocator.Create("toggle-name");
			var response = (JsonResult)await Controller.Get(locator);

			await Mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == locator));
			response.Value.ShouldBeOfType<ToggleView>();
		}
	}
}
