using System;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Projections;
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

			Mediator
				.Send(Arg.Any<GetToggleByNameRequest>())
				.Returns(new GetToggleResponse { Toggle = new ToggleView() });
		}

		[Fact]
		public async Task When_fetching_a_single_toggle_by_id()
		{
			var toggleId = Guid.NewGuid();
			var response = (JsonResult)await Controller.Get(toggleId);

			await Mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.ToggleID == toggleId));
			response.Value.ShouldBeOfType<ToggleView>();
		}

		[Fact]
		public async Task When_fetching_a_single_toggle_by_name()
		{
			var toggleName = "toggle-name";
			var response = (JsonResult)await Controller.Get(toggleName);

			await Mediator.Received().Send(Arg.Is<GetToggleByNameRequest>(req => req.Name == toggleName));
			response.Value.ShouldBeOfType<ToggleView>();
		}
	}
}
