using System;

namespace Crispin.Infrastructure
{
	public interface IAct
	{
		ToggleID AggregateID { get; set; }
		DateTime TimeStamp { get; set; }
		object Data { get; }

		void Apply(object aggregate, Aggregator applicator);
	}

	public class Act<TData> : IAct
	{
		public ToggleID AggregateID { get; set; }
		public DateTime TimeStamp { get; set; }
		public TData Data { get; set; }

		object IAct.Data => Data;

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
