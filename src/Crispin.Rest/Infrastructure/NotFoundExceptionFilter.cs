using System;
using System.Collections.Generic;
using Crispin.Conditions;
using Crispin.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crispin.Rest.Infrastructure
{
	public class NotFoundExceptionFilter : IExceptionFilter
	{
		private static readonly HashSet<Type> ManagedExceptions = new HashSet<Type>
		{
			typeof(ConditionNotFoundException),
			typeof(AggregateNotFoundException)
		};

		public void OnException(ExceptionContext context)
		{
			if (context.Exception == null)
				return;

			if (ManagedExceptions.Contains(context.Exception.GetType()) == false)
				return;

			context.ExceptionHandled = true;
			context.Result = new NotFoundResult();
		}
	}
}
