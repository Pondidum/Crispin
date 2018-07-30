using System;

namespace Crispin.Infrastructure
{
	public interface IEvent
	{
		ToggleID AggregateID { get; set; }
		DateTime TimeStamp { get; set; }
		object Data { get; }

		void Apply(object aggregate, Aggregator applicator);
	}
}
