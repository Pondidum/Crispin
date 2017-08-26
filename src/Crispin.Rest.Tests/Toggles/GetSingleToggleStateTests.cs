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
	public class GetSingleToggleStateTests : TogglesControllerTests
	{
		private readonly ToggleView _toggleView;

		public GetSingleToggleStateTests()
		{
			_toggleView = new ToggleView
			{
				ID = Guid.NewGuid(),
				Name = "toggle-1",
				Description = "the first toggle",
				State =
				{
					Anonymous = false,
					Groups = { { "group-1", true } },
					Users = { }
				},
				Tags = { "first", "dev" }
			};

			var response = new GetToggleResponse
			{
				Toggle = _toggleView
			};

			Mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse());
			Mediator
				.Send(Arg.Is<GetToggleRequest>(req => req.ToggleID == _toggleView.ID))
				.Returns(response);

			Mediator
				.Send(Arg.Any<GetToggleByNameRequest>())
				.Returns(new GetToggleResponse());
			Mediator
				.Send(Arg.Is<GetToggleByNameRequest>(req => req.Name == _toggleView.Name))
				.Returns(response);
		}

		[Fact]
		public async Task When_fetching_state_by_id()
		{
			var response = (JsonResult)await Controller.GetState(_toggleView.ID);

			await Mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.ToggleID == _toggleView.ID));
			response.Value.ShouldBeOfType<StateView>();
		}

		[Fact]
		public async Task When_fetching_state_by_id_which_doesnt_exist()
		{
			var toggleId = Guid.NewGuid();
			var response = (JsonResult)await Controller.GetState(toggleId);

			await Mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.ToggleID == toggleId));
			response.Value.ShouldBeNull();
		}

		[Fact]
		public async Task When_fetching_state_by_name()
		{
			var response = (JsonResult)await Controller.GetState(_toggleView.Name);

			await Mediator.Received().Send(Arg.Is<GetToggleByNameRequest>(req => req.Name == _toggleView.Name));
			response.Value.ShouldBeOfType<StateView>();
		}

		[Fact]
		public async Task When_fetching_state_by_name_which_doesnt_exist()
		{
			var toggleName = "some name which doesnt exist";
			var response = (JsonResult)await Controller.GetState(toggleName);

			await Mediator.Received().Send(Arg.Is<GetToggleByNameRequest>(req => req.Name == toggleName));
			response.Value.ShouldBeNull();
		}
	}
}
