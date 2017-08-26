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
		[Fact]
		public async Task When_fetching_a_single_toggle_by_id()
		{
			var view = new ToggleView();
			Mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse { Toggle = view });

			var response = (JsonResult)await Controller.Get(Guid.NewGuid());

			response.Value.ShouldBe(view);
		}
	}
}