using System;
using System.Reflection;

namespace Crispin.Infrastructure
{
	public class DirectApplicator<TEvent> : IApplicator<TEvent>
	{
		private readonly Action<object, TEvent> _apply;

		public DirectApplicator(MethodInfo method)
		{
			_apply = (a, e) => method.Invoke(a, new object[] { e });
		}

		public void Apply(object aggregate, TEvent @event) => _apply(aggregate, @event);
		public void Apply(object aggregate, Event<TEvent> @event) => _apply(aggregate, @event.Data);
	}
}
