using System;
using System.Reflection;

namespace Crispin.Infrastructure
{
	public class MetadataApplicator<TEvent> : IApplicator<TEvent>
	{
		private readonly Action<object, Event<TEvent>> _apply;

		public MetadataApplicator(MethodInfo method)
		{
			_apply = (a, e) => method.Invoke(a, new object[] { e });
		}

		public void Apply(object aggregate, TEvent @event) => throw new NotSupportedException("should never be called");
		public void Apply(object aggregate, Event<TEvent> @event) => _apply(aggregate, @event);
	}
}
