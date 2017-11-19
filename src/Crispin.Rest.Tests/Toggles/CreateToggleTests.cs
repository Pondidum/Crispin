using System.Threading.Tasks;
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
		private readonly CreateTogglesResponse _response;

		public CreateToggleTests()
		{
			_response = new CreateTogglesResponse
			{
				Toggle = Toggle.CreateNew(EditorID.Parse("123"), "one", "two").ToView()
			};
		}

		[Fact]
		public async Task When_creating_a_toggle_the_response_location_is_set()
		{
			Mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(_response);

			var result = (CreatedResult)await Controller.Post(new TogglePostRequest());

			result.ShouldSatisfyAllConditions(
				() => result.Location.ShouldBe("/toggles/id/" + _response.Toggle.ID),
				() => result.Value.ShouldBe(_response.Toggle)
			);
		}

		[Fact]
		public async Task When_creating_a_toggle_the_request_is_populated()
		{
			Mediator
				.Send(Arg.Any<CreateToggleRequest>())
				.Returns(_response);

			var model = new TogglePostRequest
			{
				Name = "the name",
				Description = "the desc"
			};

			await Controller.Post(model);

			await Mediator.Received().Send(Arg.Is<CreateToggleRequest>(
				request => request.Name == model.Name && request.Description == model.Description
			));
		}
	}
}
