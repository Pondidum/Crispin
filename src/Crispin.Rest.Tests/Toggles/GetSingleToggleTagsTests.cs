using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Handlers.GetSingle;
using Crispin.Projections;
using Crispin.Rest.Toggles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class GetSingleToggleTagsTests
	{
		private readonly ToggleView _toggleView;
		private readonly Guid _toggleID;
		private readonly IMediator _mediator;
		private readonly ToggleTagsController _controller;

		public GetSingleToggleTagsTests()
		{
			_mediator = Substitute.For<IMediator>();
			_controller = new ToggleTagsController(_mediator);

			_toggleID = Guid.NewGuid();
			_toggleView = new ToggleView
			{
				ID = ToggleID.Parse(_toggleID),
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
				Toggle = _toggleView
			};

			_mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse());
			_mediator
				.Send(Arg.Is<GetToggleRequest>(req => req.ToggleID == _toggleView.ID))
				.Returns(response);

			_mediator
				.Send(Arg.Any<GetToggleByNameRequest>())
				.Returns(new GetToggleResponse());
			_mediator
				.Send(Arg.Is<GetToggleByNameRequest>(req => req.Name == _toggleView.Name))
				.Returns(response);
		}

		[Fact]
		public async Task When_fetching_tags_by_id()
		{
			var response = (JsonResult)await _controller.GetTags(_toggleID);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.ToggleID == _toggleView.ID));
			response.Value.ShouldBeOfType<HashSet<string>>();
		}

		[Fact]
		public async Task When_fetching_tags_by_id_which_doesnt_exist()
		{
			var toggleId = Guid.NewGuid();
			var response = (JsonResult)await _controller.GetTags(toggleId);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.ToggleID == ToggleID.Parse(toggleId)));
			response.Value.ShouldBeNull();
		}

//		[Fact]
//		public async Task When_fetching_tags_by_name()
//		{
//			var response = (JsonResult)await _controller.GetTags(_toggleView.Name);
//
//			await _mediator.Received().Send(Arg.Is<GetToggleByNameRequest>(req => req.Name == _toggleView.Name));
//			response.Value.ShouldBeOfType<HashSet<string>>();
//		}
//
//		[Fact]
//		public async Task When_fetching_tags_by_name_which_doesnt_exist()
//		{
//			var toggleName = "some name which doesnt exist";
//			var response = (JsonResult)await _controller.GetTags(toggleName);
//
//			await _mediator.Received().Send(Arg.Is<GetToggleByNameRequest>(req => req.Name == toggleName));
//			response.Value.ShouldBeNull();
//		}
	}
}
