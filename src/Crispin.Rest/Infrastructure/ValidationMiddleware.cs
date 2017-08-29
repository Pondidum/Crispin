using System.Net;
using System.Threading.Tasks;
using Crispin.Infrastructure.Validation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Crispin.Rest.Infrastructure
{
	public class ValidationMiddleware
	{
		private readonly RequestDelegate _next;

		public ValidationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (ValidationException e)
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(JsonConvert.SerializeObject(new
				{
					Exception = e.GetType().Name,
					Messages = e.Messages
				}));
			}
		}
	}
}
