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

	public interface IApplicator<TEvent>
	{
		void Apply(object aggregate, TEvent @event);
		void Apply(object aggregate, Act<TEvent> @event);
	}

	public class DirectApplicator<TEvent> : IApplicator<TEvent>
	{
		private readonly Action<object, TEvent> _apply;

		public DirectApplicator(MethodInfo method)
		{
			_apply = (a, e) => method.Invoke(a, new object[] { e });
		}

		public void Apply(object aggregate, TEvent @event) => _apply(aggregate, @event);
		public void Apply(object aggregate, Act<TEvent> @event) => _apply(aggregate, @event.Data);
	}

	public class MetadataApplicator<TEvent> : IApplicator<TEvent>
	{
		private readonly Action<object, Act<TEvent>> _apply;

		public MetadataApplicator(MethodInfo method)
		{
			_apply = (a, e) => method.Invoke(a, new object[] { e });
		}

		public void Apply(object aggregate, TEvent @event) => throw new NotSupportedException("should never be called");
		public void Apply(object aggregate, Act<TEvent> @event) => _apply(aggregate, @event);
	}
}
