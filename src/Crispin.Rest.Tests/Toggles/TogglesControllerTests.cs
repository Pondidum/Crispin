using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Projections;
using Crispin.Rest.Toggles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class TogglesControllerTests
	{
		private readonly IMediator _mediator;
		private readonly TogglesController _controller;

		public TogglesControllerTests()
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

		[Fact]
		public async Task When_creating_a_toggle_the_response_location_is_set()
		{
			var response = new CreateTogglesResponse { ToggleID = Guid.NewGuid() };
			_mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(response);

			var result = await _controller.Post(new TogglePostRequest()) as CreatedResult;

			result.Location.ShouldBe("/toggles/id/" + response.ToggleID);
		}

		[Fact]
		public async Task When_creating_a_toggle_the_request_is_populated()
		{
			_mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(new CreateTogglesResponse());

			var model = new TogglePostRequest
			{
				Name = "the name",
				Description = "the desc"
			};

			await _controller.Post(model);

			_mediator.Received().Send(Arg.Is<CreateToggleRequest>(
				request => request.Name == model.Name && request.Description == model.Description
			));
		}

		[Fact]
		public async Task When_fetching_a_single_toggle()
		{
			var view = new ToggleView();
			_mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse { Toggle = view });

			var response = (JsonResult)await _controller.Get(Guid.NewGuid());

			response.Value.ShouldBe(view);
		}
	}
}
