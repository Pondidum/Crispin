using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Handlers.AddCondition;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.RemoveCondition;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Api.Toggles
{
	[Route("api/toggles")]
	public class ToggleConditionsController : Controller
	{
		private readonly IMediator _mediator;

		public ToggleConditionsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		private EditorID GetEditor() => EditorID.Parse("TestApiUser"); //User.Identity.Name

		[Route("id/{id}/conditions")]
		[Route("name/{id}/conditions")]
		[HttpGet]
		public async Task<IActionResult> GetConditions(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.Conditions);
		}

		[Route("id/{id}/conditions/{condition}")]
		[Route("name/{id}/conditions/{condition}")]
		[HttpGet]
		public async Task<IActionResult> GetConditions(ToggleLocator id, ConditionID condition)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.Conditions?.SingleOrDefault(c => c.ID == condition));
		}

		[Route("id/{id}/conditions")]
		[Route("name/{id}/conditions")]
		[HttpPost]
		public async Task<IActionResult> AddCondition(ToggleLocator id, [FromBody] ConditionDto condition)
		{
			var request = new AddToggleConditionRequest(GetEditor(), id, condition);
			var response = await _mediator.Send(request);

			var uri = Url.Action(nameof(GetConditions), new
			{
				id = response.ToggleID,
				condition = response.Condition.ID
			});

			return Created(uri, response.Condition);
		}

		[Route("id/{id}/conditions/{condition}")]
		[Route("name/{id}/conditions/{condition}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteCondition(ToggleLocator id, ConditionID condition)
		{
			var request = new RemoveToggleConditionRequest(GetEditor(), id, condition);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Conditions);
		}
	}

	public class ConditionDto : Dictionary<string, object>
	{
	}
}
