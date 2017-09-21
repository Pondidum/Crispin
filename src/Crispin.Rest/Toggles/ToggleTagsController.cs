using System;
using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.UpdateTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Toggles
{
	[Route("Toggles")]
	public class ToggleTagsController : Controller
	{
		private readonly IMediator _mediator;

		public ToggleTagsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Route("id/{id}/tags")]
		[HttpGet]
		public async Task<IActionResult> GetTags(Guid id)
		{
			var request = new GetToggleRequest(ToggleID.Parse(id));
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.Tags);
		}


		[Route("id/{id}/tags/{tagName}")]
		[HttpPut]
		public async Task<IActionResult> PutTag(Guid id, string tagName)
		{
			var request = new AddToggleTagRequest(ToggleLocator.Create(ToggleID.Parse(id)), tagName);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Tags);
		}

		[Route("id/{id}/tags/{tagName}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteTag(Guid id, string tagName)
		{
			var request = new RemoveToggleTagRequest(ToggleLocator.Create(ToggleID.Parse(id)), tagName);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Tags);
		}

		[Route("name/{toggleName}/tags/{tagName}")]
		[HttpPut]
		public async Task<IActionResult> PutTag(string toggleName, string tagName)
		{
			var request = new AddToggleTagRequest(ToggleLocator.Create(toggleName), tagName);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Tags);
		}

		[Route("name/{toggleName}/tags/{tagName}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteTag(string toggleName, string tagName)
		{
			var request = new RemoveToggleTagRequest(ToggleLocator.Create(toggleName), tagName);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Tags);
		}
	}
}
