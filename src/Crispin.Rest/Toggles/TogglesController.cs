using System;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Handlers.Create;
using Crispin.Handlers.GetAll;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.UpdateState;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Toggles
{
	[Route("[controller]")]
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
			var request = new GetToggleRequest(id);
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

		[Route("id/{id}/state")]
		[HttpGet]
		public async Task<IActionResult> GetState(Guid id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.State);
		}

		[Route("id/{id}/state")]
		[HttpPost]
		public async Task<IActionResult> PostState(Guid id, [FromBody] UpdateStateModel model)
		{
			var request = new UpdateToggleStateRequest(
				id,
				model.Anonymous,
				model.Groups,
				model.Users
			);

			var response = await _mediator.Send(request);

			return new JsonResult(response); //??
		}

		[Route("name/{name}/state")]
		[HttpGet]
		public async Task<IActionResult> GetState(string name)
		{
			var request = new GetToggleByNameRequest(name);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.State);
		}

		[Route("id/{id}/tags")]
		[HttpGet]
		public async Task<IActionResult> GetTags(Guid id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.Tags);
		}

		[Route("name/{name}/tags")]
		[HttpGet]
		public async Task<IActionResult> GetTags(string name)
		{
			var request = new GetToggleByNameRequest(name);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.Tags);
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
