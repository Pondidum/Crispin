using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Handlers.GetSingle;
using Crispin.Projections;
using Crispin.Rest.Toggles;
using Crispin.Views;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class GetSingleToggleTagsTests
	{
		private readonly IMediator _mediator;
		private readonly ToggleTagsController _controller;
		private readonly ToggleLocator _validLocator;
		private readonly ToggleLocator _invalidLocator;

		public GetSingleToggleTagsTests()
		{
			_mediator = Substitute.For<IMediator>();
			_controller = new ToggleTagsController(_mediator);

			_validLocator = ToggleLocator.Create("i exist");
			_invalidLocator = ToggleLocator.Create("i dont exist");

			var toggleView = new ToggleView
			{
				ID = ToggleID.CreateNew(),
				Name = "toggle-1",
				Description = "the first toggle",
				Tags = { "first", "dev" }
			};

			var response = new GetToggleResponse
			{
				Toggle = toggleView
			};

			_mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse());
			_mediator
				.Send(Arg.Is<GetToggleRequest>(req => req.Locator == _validLocator))
				.Returns(response);
		}

		[Fact]
		public async Task When_fetching_tags_by_id()
		{
			var response = (JsonResult)await _controller.GetTags(_validLocator);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == _validLocator));
			response.Value.ShouldBeOfType<HashSet<string>>();
		}

		[Fact]
		public async Task When_fetching_tags_by_id_which_doesnt_exist()
		{
			var response = (JsonResult)await _controller.GetTags(_invalidLocator);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == _invalidLocator));
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
