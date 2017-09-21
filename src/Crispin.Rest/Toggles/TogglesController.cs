﻿using System;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Handlers.Create;
using Crispin.Handlers.GetAll;
using Crispin.Handlers.GetSingle;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Crispin.Rest.Toggles
{
	[Route("Toggles")]
	public class TogglesController : Controller
	{
		private readonly IMediator _mediator;

		public TogglesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Route("")]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var request = new GetAllTogglesRequest();
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggles);
		}

		[Route("id/{id}")]
		[HttpGet]
		public async Task<IActionResult> Get(Guid id)
		{
			var request = new GetToggleRequest(ToggleID.Parse(id));
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle);
		}

		[Route("name/{name}")]
		[HttpGet]
		public async Task<IActionResult> Get(string name)
		{
			var request = new GetToggleByNameRequest(name);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle);
		}

		[Route("")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] TogglePostRequest model)
		{
			var request = new CreateToggleRequest("???", model.Name, model.Description);
			var response = await _mediator.Send(request);

			return Created("/toggles/id/" + response.ToggleID, null);
		}
	}
}
