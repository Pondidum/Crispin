using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.UpdateTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Api.Toggles
{
	[Route("api/toggles")]
	public class ToggleTagsController : Controller
	{
		private readonly IMediator _mediator;

		public ToggleTagsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		private EditorID GetEditor() => EditorID.Parse("TestApiUser");		//User.Identity.Name

		[Route("id/{id}/tags")]
		[Route("name/{id}/tags")]
		[HttpGet]
		public async Task<IActionResult> GetTags(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.Tags);
		}

		[Route("id/{id}/tags/{tagName}")]
		[Route("name/{id}/tags/{tagName}")]
		[HttpPut]
		public async Task<IActionResult> PutTag(ToggleLocator id, string tagName)
		{
			var request = new AddToggleTagRequest(GetEditor(), id, tagName);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Tags);
		}

		[Route("id/{id}/tags/{tagName}")]
		[Route("name/{id}/tags/{tagName}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteTag(ToggleLocator id, string tagName)
		{
			var request = new RemoveToggleTagRequest(GetEditor(), id, tagName);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Tags);
		}
	}
}
