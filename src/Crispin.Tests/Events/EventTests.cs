using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Crispin.Conditions;
using Crispin.Events;
using Crispin.Infrastructure;
using Newtonsoft.Json;
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

		[Theory]
		[MemberData(nameof(AllEvents))]
		public void Events_serialize_and_deserialize_correctly(Type eventType)
		{
			var fixture = new Fixture();
			fixture.Register(() => new Dictionary<string, object>
			{
				{ ConditionBuilder.TypeKey, "wat" }
			});
			var instance = new SpecimenContext(fixture).Resolve(eventType);

			var json = JsonConvert.SerializeObject(instance);
			var loaded = JsonConvert.DeserializeObject(json, eventType);

			var conditions = eventType
				.GetProperties()
				.Select(p =>
				{
					var invoke = new Func<object, object>(target => p.GetGetMethod().Invoke(target, new object[0]));
					var loadedProp = invoke(loaded);
					var originalProp = invoke(instance);

					return new Action(() => loadedProp.ShouldBe(originalProp, p.Name));
				})
				.ToArray();

			loaded.ShouldSatisfyAllConditions(conditions);
		}

		[Fact]
		public void There_are_events()
		{
			AllEvents.ShouldNotBeEmpty();
		}
	}
}
