using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class RequestRulesTests
	{
		[Fact]
		public void There_should_be_requests() => AllRequests.ShouldNotBeEmpty();

		[Theory]
		[MemberData(nameof(AllRequests))]
		public void All_requests_must_not_have_a_toggle_id(Type request) => request
			.GetProperties()
			.Where(prop => prop.PropertyType == typeof(ToggleID)).ShouldBeEmpty();

		public static IEnumerable<object[]> AllRequests => typeof(ToggleLocator)
			.Assembly
			.GetExportedTypes()
			.Where(t => t.IsClass && t.IsAbstract == false)
			.Where(type => Implements(type, typeof(IRequest<>)))
			.Select(type => new object[] { type });

		private static bool Implements(Type type, Type lookFor) => type
			.GetInterfaces()
			.Where(i => i.IsGenericType)
			.Select(i => i.GetGenericTypeDefinition())
			.Contains(lookFor);
	}
}
