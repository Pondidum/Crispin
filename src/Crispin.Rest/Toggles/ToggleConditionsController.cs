using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Toggles
{
	[Route("toggles")]
	public class ToggleConditionsController
	{
		private readonly IMediator _mediator;

		public ToggleConditionsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Route("id/{id}/conditions")]
		[Route("name/{id}/conditions")]
		[HttpGet]
		public async Task<IActionResult> GetConditions(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle?.Conditions);
		}
	}
}
