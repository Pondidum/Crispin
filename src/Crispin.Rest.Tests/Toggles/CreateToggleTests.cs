using System;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Handlers.Create;
using Crispin.Rest.Toggles;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class CreateToggleTests : TogglesControllerTests
	{
		[Fact]
		public async Task When_creating_a_toggle_the_response_location_is_set()
		{
			var response = new CreateTogglesResponse { ToggleID = Guid.NewGuid() };
			Mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(response);

			var result = await Controller.Post(new TogglePostRequest()) as CreatedResult;

			result.Location.ShouldBe("/toggles/id/" + response.ToggleID);
		}

		[Fact]
		public async Task When_creating_a_toggle_the_request_is_populated()
		{
			Mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(new CreateTogglesResponse());

			var model = new TogglePostRequest
			{
				Name = "the name",
				Description = "the desc"
			};

			await Controller.Post(model);

			Mediator.Received().Send(Arg.Is<CreateToggleRequest>(
				request => request.Name == model.Name && request.Description == model.Description
			));
		}
	}
}