using Crispin.Conditions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crispin.Rest.Infrastructure
{
	public class NotFoundExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			var exception = context.Exception as ConditionNotFoundException;

			if (exception == null)
				return;

			context.ExceptionHandled = true;
			context.Result = new NotFoundResult();
		}
	}
}
