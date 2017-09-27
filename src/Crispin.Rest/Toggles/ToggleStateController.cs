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

		private EditorID GetEditor() => EditorID.Parse("TestApiUser");		//User.Identity.Name

		[Route("id/{id}/state")]
		[Route("name/{id}/state")]
		[HttpGet]
		public async Task<IActionResult> GetState(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.State);
		}

		[Route("id/{id}/state/default/")]
		[Route("name/{id}/state/default/")]
		[HttpPut]
		public async Task<IActionResult> PutStateDefault(ToggleLocator id, [FromBody] StatePutRequest model)
		{
			var request = new UpdateToggleStateRequest(GetEditor(), id) { Anonymous = model.State };
			var response = await _mediator.Send(request);

			return new JsonResult(response.State);
		}

		[Route("id/{id}/state/users/{userid}")]
		[Route("name/{id}/state/users/{userid}")]
		[HttpPut]
		public async Task<IActionResult> PutStateUser(ToggleLocator id, UserID userID, [FromBody] StatePutRequest model)
		{
			var request = new UpdateToggleStateRequest(GetEditor(), id)
			{
				Users = { { userID, model.State } }
			};
			var response = await _mediator.Send(request);

			return new JsonResult(response.State);
		}

		[Route("id/{id}/state/groups/{groupid}")]
		[Route("name/{id}/state/groups/{groupid}")]
		[HttpPut]
		public async Task<IActionResult> PutStateGroup(ToggleLocator id, GroupID groupid, [FromBody] StatePutRequest model)
		{
			var request = new UpdateToggleStateRequest(GetEditor(), id)
			{
				Groups = { { groupid, model.State } }
			};
			var response = await _mediator.Send(request);

			return new JsonResult(response.State);
		}

		[Route("id/{id}/state/users/{userid}")]
		[Route("name/{id}/state/users/{userid}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteStateUser(ToggleLocator id, UserID userID)
		{
			var request = new UpdateToggleStateRequest(GetEditor(), id)
			{
				Users = { { userID, null } }
			};
			var response = await _mediator.Send(request);

			return new JsonResult(response.State);
		}

		[Route("id/{id}/state/groups/{groupid}")]
		[Route("name/{id}/state/groups/{groupid}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteStateGroup(ToggleLocator id, GroupID groupid)
		{
			var request = new UpdateToggleStateRequest(GetEditor(), id)
			{
				Groups = { { groupid, null } }
			};
			var response = await _mediator.Send(request);

			return new JsonResult(response.State);
		}
	}
}
