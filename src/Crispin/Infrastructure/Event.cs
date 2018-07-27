using System;

namespace Crispin.Infrastructure
{
	public interface IEvent
	{
		ToggleID AggregateID { get; set; }
		DateTime TimeStamp { get; set; }
		object Data { get; }
	}

	public class Act<TData> : IEvent
	{
		public ToggleID AggregateID { get; set; }
		public DateTime TimeStamp { get; set; }
		object IEvent.Data => Data;

		public TData Data { get; set; }

		public void Apply(AggregateRoot aggregate, Aggregator applicator)
		{
			var handler = applicator.For<TData>();

			if (handler is DirectApplicator<TData>)
				handler.Apply(aggregate, Data);
			else
				handler.Apply(aggregate, this);
		}
	}

	public abstract class Event : IEvent
	{
		public ToggleID AggregateID { get; set; }
		public DateTime TimeStamp { get; set; }
		object IEvent.Data => this;
	}
}
