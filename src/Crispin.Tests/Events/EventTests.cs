using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Events
{
	public class EventTests
	{
		public static IEnumerable<object[]> AllEvents => typeof(Event)
			.GetTypeInfo()
			.Assembly
			.GetExportedTypes()
			.Where(t => t.GetTypeInfo().IsAbstract == false)
			.Where(t => typeof(Event).IsAssignableFrom(t))
			.Select(t => new object[] { t });

		[Theory]
		[MemberData(nameof(AllEvents))]
		public void All_events_override_tostring(Type eventType)
		{
			var overridden = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
			var method = eventType.GetMethod(nameof(Event.ToString), overridden);

			method.ShouldNotBeNull($"{eventType.Name} should override ToString(), but does not.");
		}
	}
}
