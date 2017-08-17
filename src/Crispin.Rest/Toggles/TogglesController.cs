using System.Threading.Tasks;
using Crispin.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

			return new JsonResult(response);
		}
	}
}
