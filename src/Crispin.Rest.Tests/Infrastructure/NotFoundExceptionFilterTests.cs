using System.Collections.Generic;
using System.Threading;
using Crispin.Conditions;
using Crispin.Rest.Infrastructure;
using Crispin.Rest.Tests.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Infrastructure
{
	public class NotFoundExceptionFilterTests
	{
		private readonly NotFoundExceptionFilter _filter;
		private readonly ExceptionContext _context;

		public NotFoundExceptionFilterTests()
		{
			var actionContext = new ActionContext
			{
				HttpContext = new DefaultHttpContext(),
				RouteData = new RouteData(),
				ActionDescriptor = new ActionDescriptor(),
				ModelState = { }
			};

			_filter = new NotFoundExceptionFilter();
			_context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
			{
				Result = new OkResult(),
				ExceptionHandled = false
			};
		}

		[Fact]
		public void When_there_is_no_exception()
		{
			_filter.OnException(_context);
			_context.Result.ShouldBeOfType<OkResult>();
		}

		[Fact]
		public void When_the_exception_is_not_a_condition_not_found_exception()
		{
			_context.Exception = new ExpectedException();

			_filter.OnException(_context);

			_context.ShouldSatisfyAllConditions(
				() => _context.Exception.ShouldBeOfType<ExpectedException>(),
				() => _context.ExceptionHandled.ShouldBeFalse(),
				() => _context.Result.ShouldBeOfType<OkResult>()
			);
		}

		[Fact]
		public void When_the_exception_is_a_condition_not_found_exception()
		{
			_context.Exception = new ConditionNotFoundException(ConditionID.Parse(123));

			_filter.OnException(_context);

			_context.ShouldSatisfyAllConditions(
				() => _context.ExceptionHandled.ShouldBeTrue(),
				() => _context.Result.ShouldBeOfType<NotFoundResult>()
			);
		}
	}
}
