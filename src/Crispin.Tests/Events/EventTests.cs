using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crispin.Events;
using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Events
{
	public class EventTests
	{
		public static IEnumerable<object[]> AllEvents => typeof(ToggleCreated)
			.GetTypeInfo()
			.Assembly
			.GetExportedTypes()
			.Where(t => t.GetTypeInfo().IsAbstract == false)
			.Where(t => t.Namespace == typeof(ToggleCreated).Namespace)
			.Select(t => new object[] { t });

		[Theory]
		[MemberData(nameof(AllEvents))]
		public void All_events_override_tostring(Type eventType)
		{
			var overridden = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
			var method = eventType.GetMethod(nameof(ToString), overridden);

			method.ShouldNotBeNull($"{eventType.Name} should override ToString(), but does not.");
		}

		[Fact]
		public void There_are_events()
		{
			AllEvents.ShouldNotBeEmpty();
		}
	}
}
