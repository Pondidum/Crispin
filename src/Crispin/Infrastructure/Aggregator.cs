using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Crispin.Infrastructure
{
	public class Aggregator
	{
		private readonly Dictionary<Type, Action<object, Event>> _handlers;

		public Aggregator(Type aggregate) : this(MethodsFor(aggregate))
		{
		}

		public Aggregator(IEnumerable<MethodInfo> methods)
		{
			_handlers = new Dictionary<Type, Action<object, Event>>();
			foreach (var method in methods)
			{
				var eventType = method.GetParameters().Single().ParameterType;

				_handlers.Add(eventType, (a, e) => method.Invoke(a, new object[] { e }));
			}
		}

		public void Apply(object aggregate, IEnumerable<Event> @events)
		{
			@events.Each(e => Apply(aggregate, e));
		}

		public void Apply<TEvent>(object aggregate, TEvent @event) where TEvent : Event
		{
			if (_handlers.TryGetValue(@event.GetType(), out var handler))
				handler(aggregate, @event);
		}

		private static IEnumerable<MethodInfo> MethodsFor(Type aggregate) => aggregate
			.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.Where(m => m.Name == "Apply")
			.Where(m => m.GetParameters().Length == 1);
	}
}