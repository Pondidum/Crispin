using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crispin.Rest.Infrastructure
{
	public class JsonNotFoundActionFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			var json = context.Result as JsonResult;

			if (json != null && json.Value == null)
				context.Result = new NotFoundResult();
		}
	}
}
