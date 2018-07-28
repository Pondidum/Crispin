using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Crispin.Infrastructure
{
	public class Aggregator
	{
		private readonly Dictionary<Type, object> _handlers;

		public Aggregator(Type aggregate) : this(MethodsFor(aggregate))
		{
		}

		public Aggregator(IEnumerable<MethodInfo> methods)
		{
			_handlers = new Dictionary<Type, object>();
			foreach (var method in methods)
			{
				var eventType = method.GetParameters().Single().ParameterType;
				object applicator;

				if (eventType.Closes(typeof(Act<>)))
				{
					eventType = eventType.GetGenericArguments().Single();
					applicator = typeof(MetadataApplicator<>).CloseAndBuildAs<object>(method, eventType);
				}
				else
				{
					applicator = typeof(DirectApplicator<>).CloseAndBuildAs<object>(method, eventType);
				}

				_handlers[eventType] = applicator;
			}
		}

		public IApplicator<TEvent> For<TEvent>() => _handlers.TryGetValue(typeof(TEvent), out var handler)
			? handler as IApplicator<TEvent>
			: null;

		private static IEnumerable<MethodInfo> MethodsFor(Type aggregate) => aggregate
			.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.Where(m => m.Name == "Apply")
			.Where(m => m.GetParameters().Length == 1);
	}
}
