using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Api.System
{
	[Route("api/system")]
	public class SystemController : Controller
	{
		[Route("info")]
		[HttpGet]
		public async Task<IActionResult> Get(ToggleLocator id)
		{
			return new JsonResult(new
			{
				ConditionTypes = ConditionBuilder.AvailableTypes().OrderBy(n => n)
			});
		}
	}
}
