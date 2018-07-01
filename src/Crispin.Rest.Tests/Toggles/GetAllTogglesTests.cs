using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers.GetAll;
using Crispin.Rest.Toggles;
using Crispin.Views;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class GetAllTogglesTests
	{
		private readonly IMediator _mediator;
		private readonly TogglesController _controller;

		public GetAllTogglesTests()
		{
			_mediator = Substitute.For<IMediator>();
			_controller = new TogglesController(_mediator);
		}

		[Fact]
		public async Task When_getting_all_toggles_and_there_are_none()
		{
			_mediator
				.Send(Arg.Any<GetAllTogglesRequest>())
				.Returns(new GetAllTogglesResponse());

			var response = await _controller.Get() as JsonResult;

			((IEnumerable<ToggleView>)response.Value).ShouldBeEmpty();
		}

		[Fact]
		public async Task When_getting_all_toggles_and_there_are_some()
		{
			var response = new GetAllTogglesResponse();
			response.Toggles = new[] { new ToggleView(), new ToggleView(), new ToggleView() };

			_mediator
				.Send(Arg.Any<GetAllTogglesRequest>())
				.Returns(response);

			var jsonResult = await _controller.Get() as JsonResult;

			jsonResult.Value.ShouldBe(response.Toggles);
		}
	}
}
