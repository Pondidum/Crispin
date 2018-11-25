using System.Threading.Tasks;
using Crispin.Handlers.ChangeConditionMode;
using Crispin.Handlers.ChangeDescription;
using Crispin.Handlers.Create;
using Crispin.Handlers.GetAll;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.Rename;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Api.Toggles
{
	[Route("api/toggles")]
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
		[Route("name/{id}")]
		[HttpGet]
		public async Task<IActionResult> Get(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(response.Toggle);
		}

		[Route("id/{id}/name")]
		[Route("name/{id}/name")]
		[HttpGet]
		public async Task<IActionResult> GetName(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(new { response.Toggle?.Name });
		}

		[Route("id/{id}/description")]
		[Route("name/{id}/description")]
		[HttpGet]
		public async Task<IActionResult> GetDescription(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(new { response.Toggle?.Description });
		}

		[Route("id/{id}/conditionMode")]
		[Route("name/{id}/conditionMode")]
		[HttpGet]
		public async Task<IActionResult> GetConditionMode(ToggleLocator id)
		{
			var request = new GetToggleRequest(id);
			var response = await _mediator.Send(request);

			return new JsonResult(new { response.Toggle?.ConditionMode });
		}

		[Route("")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] TogglePostRequest model)
		{
			var editor = EditorID.Parse("TestApiUser"); //User.Identity.Name
			var request = new CreateToggleRequest(editor, model.Name, model.Description);
			var response = await _mediator.Send(request);

			var uri = Url.Action(nameof(Get), new { id = response.Toggle.ID });

			return Created(uri, response.Toggle);
		}

		[Route("id/{id}/name")]
		[Route("name/{id}/name")]
		[HttpPut]
		public async Task<IActionResult> PutName(ToggleLocator id, [FromBody] ToggleNamePutRequest model)
		{
			var editor = EditorID.Parse("TestApiUser"); //User.Identity.Name
			var request = new RenameToggleRequest(editor, id, model.Name);
			var response = await _mediator.Send(request);

			return new JsonResult(response);
		}

		[Route("id/{id}/description")]
		[Route("name/{id}/description")]
		[HttpPut]
		public async Task<IActionResult> PutDescription(ToggleLocator id, [FromBody] ToggleDescriptionPutRequest model)
		{
			var editor = EditorID.Parse("TestApiUser"); //User.Identity.Name
			var request = new ChangeToggleDescriptionRequest(editor, id, model.Description);
			var response = await _mediator.Send(request);

			return new JsonResult(response);
		}

		[Route("id/{id}/conditionMode")]
		[Route("name/{id}/conditionMode")]
		[HttpPut]
		public async Task<IActionResult> PutConditionMode(ToggleLocator id, [FromBody] ConditionModePutRequest model)
		{
			var editor = EditorID.Parse("TestApiUser"); //User.Identity.Name
			var request = new ChangeConditionModeRequest(editor, id, model.ConditionMode);
			var response = await _mediator.Send(request);

			return new JsonResult(response);
		}
	}
}
