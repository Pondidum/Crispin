using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Crispin.Rest.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Infrastructure
{
	public class JsonNotFoundActionFilterTests
	{
		private readonly JsonNotFoundActionFilter _filter;
		private readonly ActionExecutedContext _context;

		public JsonNotFoundActionFilterTests()
		{
			_filter = new JsonNotFoundActionFilter();

			var actionContext = new ActionContext
			{
				HttpContext = new DefaultHttpContext(),
				RouteData = new RouteData(),
				ActionDescriptor = new ActionDescriptor(),
				ModelState = { }
			};

			_context = new ActionExecutedContext(
				actionContext,
				new List<IFilterMetadata>(),
				null);
		}

		[Fact]
		public void When_the_response_is_not_json()
		{
			_context.Result = new ContentResult();

			_filter.OnActionExecuted(_context);

			_context.Result.ShouldBeOfType<ContentResult>();
		}

		[Fact]
		public void When_the_response_is_populated_json()
		{
			_context.Result = new JsonResult(new
			{
				Name = "Dave"
			});

			_filter.OnActionExecuted(_context);

			_context.Result.ShouldBeOfType<JsonResult>();
		}

		[Fact]
		public void When_the_response_is_null_json()
		{
			_context.Result = new JsonResult(null);

			_filter.OnActionExecuted(_context);

			_context.Result.ShouldBeOfType<NotFoundResult>();
		}
	}
}
