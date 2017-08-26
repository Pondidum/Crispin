using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Projections;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class GetAllTogglesTests : TogglesControllerTests
	{
		[Fact]
		public async Task When_getting_all_toggles_and_there_are_none()
		{
			Mediator
				.Send(Arg.Any<GetAllTogglesRequest>())
				.Returns(new GetAllTogglesResponse());

			var response = await Controller.Get() as JsonResult;

			((IEnumerable<ToggleView>)response.Value).ShouldBeEmpty();
		}

		[Fact]
		public async Task When_getting_all_toggles_and_there_are_some()
		{
			var response = new GetAllTogglesResponse();
			response.Toggles = new[] { new ToggleView(), new ToggleView(), new ToggleView() };

			Mediator
				.Send(Arg.Any<GetAllTogglesRequest>())
				.Returns(response);

			var jsonResult = await Controller.Get() as JsonResult;

			jsonResult.Value.ShouldBe(response.Toggles);
		}
	}
}
