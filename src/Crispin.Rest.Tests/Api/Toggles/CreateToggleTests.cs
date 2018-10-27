using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Crispin.Rest.Api.Toggles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Api.Toggles
{
	public class CreateToggleTests
	{
		private readonly IMediator _mediator;
		private readonly TogglesController _controller;
		private readonly CreateTogglesResponse _response;

		public CreateToggleTests()
		{
			_mediator = Substitute.For<IMediator>();
			_controller = new TogglesController(_mediator);
			_response = new CreateTogglesResponse
			{
				Toggle = Toggle.CreateNew(EditorID.Parse("123"), "one", "two").ToView()
			};
		}

		[Fact]
		public async Task When_creating_a_toggle_the_response_location_is_set()
		{
			_mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(_response);

			var result = (CreatedResult)await _controller.Post(new TogglePostRequest());

			result.ShouldSatisfyAllConditions(
				() => result.Location.ShouldBe("/api/toggles/id/" + _response.Toggle.ID),
				() => result.Value.ShouldBe(_response.Toggle)
			);
		}

		[Fact]
		public async Task When_creating_a_toggle_the_request_is_populated()
		{
			_mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(_response);

			var model = new TogglePostRequest
			{
				Name = "the name",
				Description = "the desc"
			};

			await _controller.Post(model);

			await _mediator.Received().Send(Arg.Is<CreateToggleRequest>(
				request => request.Name == model.Name && request.Description == model.Description
			));
		}
	}
}
