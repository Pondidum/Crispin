using System;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.UpdateState;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Toggles
{
	[Route("Toggles")]
	public class ToggleStateController : Controller
	{
		private readonly IMediator _mediator;

		public ToggleStateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Route("id/{id}/state")]
		[Route("name/{id}/state")]
		[HttpGet]
		public async Task<IActionResult> GetState(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.State);
		}

		[Route("id/{id}/state")]
		[Route("name/{id}/state")]
		[HttpPut]
		public async Task<IActionResult> PostState(ToggleLocator id, [FromBody] UpdateStateModel model)
		{
			var request = new UpdateToggleStateRequest(id)
			{
				Anonymous = model.Anonymous.AsState(),
				Groups = model.Groups.ToDictionary(p => GroupID.Parse(p.Key), p => p.Value.AsState()),
				Users = model.Users.ToDictionary(p => UserID.Parse(p.Key), p => p.Value.AsState())
			};

			var response = await _mediator.Send(request);

			return new JsonResult(response.State);
		}

		[Route("id/{id}/state")]
		[Route("name/{id}/state")]
		[HttpDelete]
		public async Task<IActionResult> DeleteState(ToggleLocator id, [FromBody] DeleteStateModel model)
		{
			var request = new UpdateToggleStateRequest(id)
			{
				Groups = model.Groups.ToDictionary(GroupID.Parse, p => (States?)null),
				Users = model.Users.ToDictionary(UserID.Parse, p => (States?)null)
			};

			var response = await _mediator.Send(request);

			return new JsonResult(response.State);
		}
	}
}
