using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Rest.Toggles;
using Crispin.Views;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class GetSingleToggleTests
	{
		private readonly IMediator _mediator;
		private readonly TogglesController _controller;

		public GetSingleToggleTests()
		{
			_mediator = Substitute.For<IMediator>();
			_controller = new TogglesController(_mediator);
			_mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse { Toggle = new ToggleView() });
		}

		[Fact]
		public async Task When_fetching_a_single_toggle_by_id()
		{
			var locator = ToggleLocator.Create(ToggleID.CreateNew());
			var response = (JsonResult)await _controller.Get(locator);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == locator));
			response.Value.ShouldBeOfType<ToggleView>();
		}

		[Fact]
		public async Task When_fetching_a_single_toggle_by_name()
		{
			var locator = ToggleLocator.Create("toggle-name");
			var response = (JsonResult)await _controller.Get(locator);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == locator));
			response.Value.ShouldBeOfType<ToggleView>();
		}
	}
}
