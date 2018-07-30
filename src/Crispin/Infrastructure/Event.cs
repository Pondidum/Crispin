using System;

namespace Crispin.Infrastructure
{
	public class Event<TData> : IEvent
	{
		public ToggleID AggregateID { get; set; }
		public DateTime TimeStamp { get; set; }
		public TData Data { get; set; }

		object IEvent.Data => Data;

		public void Apply(object aggregate, Aggregator applicator)
		{
			var handler = applicator.For<TData>();

			if (handler is DirectApplicator<TData>)
				handler.Apply(aggregate, Data);
			else
				handler.Apply(aggregate, this);
		}
	}
}
