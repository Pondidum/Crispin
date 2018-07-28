using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure.Storage
{
	public static class AggregateBuilder
	{
		public static T Build<T>(T instance, IEnumerable<IAct> events) => Build(() => instance, events);
	
		public static T Build<T>(Func<T> createBlank, IEnumerable<IAct> events)
		{
			var instance = createBlank();
			var applicator = new Aggregator(typeof(T));
			events.Each(e => e.Apply(instance, applicator));
				
			return instance;
		}
	}
}
