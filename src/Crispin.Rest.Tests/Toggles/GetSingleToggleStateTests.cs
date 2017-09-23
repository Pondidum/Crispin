using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Projections;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class GetSingleToggleStateTests : ToggleStateControllerTests
	{
		private readonly ToggleLocator _validLocator;
		private readonly ToggleLocator _invalidLocator;

		public GetSingleToggleStateTests()
		{
			_validLocator = ToggleLocator.Create("i exist");
			_invalidLocator = ToggleLocator.Create("i dont exist");

			var toggleView = new ToggleView
			{
				ID = ToggleID.CreateNew(),
				Name = "toggle-1",
				Description = "the first toggle",
				State =
				{
					Anonymous = States.Off,
					Groups = { { GroupID.Parse("group-1"), States.On } },
					Users = { }
				},
				Tags = { "first", "dev" }
			};

			var response = new GetToggleResponse
			{
				Toggle = toggleView
			};

			Mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse());
			Mediator
				.Send(Arg.Is<GetToggleRequest>(req => req.Locator == _validLocator))
				.Returns(response);

		}

		[Fact]
		public async Task When_fetching_state_by_id()
		{
			var response = (JsonResult)await Controller.GetState(_validLocator);

			await Mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == _validLocator));
			response.Value.ShouldBeOfType<StateView>();
		}

		[Fact]
		public async Task When_fetching_state_by_id_which_doesnt_exist()
		{
			var response = (JsonResult)await Controller.GetState(_invalidLocator);

			await Mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == _invalidLocator));
			response.Value.ShouldBeNull();
		}
	}
}
