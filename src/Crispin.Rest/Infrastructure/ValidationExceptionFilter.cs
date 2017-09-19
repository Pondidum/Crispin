using System.Net;
using Crispin.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crispin.Rest.Infrastructure
{
	public class ValidationExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			var validationException = context.Exception as ValidationException;

			if (validationException == null)
				return;

			var validationResponse = new ValidationResponse
			{
				Exception = validationException.GetType().Name,
				Messages = validationException.Messages
			};

			var result = new JsonResult(validationResponse)
			{
				StatusCode = (int)HttpStatusCode.BadRequest
			};

			context.ExceptionHandled = true;
			context.Result = result;
		}
	}
}
