using System;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	public abstract class AggregateRoot<TIdentity> : IEvented
	{
		private readonly List<IEvent> _pendingEvents;
		private readonly Aggregator _applicator;

		protected AggregateRoot()
		{
			_applicator = new Aggregator(GetType());
			_pendingEvents = new List<IEvent>();
		}

		public ToggleID ID { get; protected set; }

		protected void ApplyEvent<TEvent>(TEvent @event)
		{
			var act = new Event<TEvent>
			{
				AggregateID = ID,
				TimeStamp = DateTime.Now,
				Data = @event
			};

			act.Apply(this, _applicator);
			act.AggregateID = ID; // the handler might have set the ID (e.g. createdEvents)

			_pendingEvents.Add(act);
		}

		IEnumerable<IEvent> IEvented.GetPendingEvents() => _pendingEvents;
		void IEvented.ClearPendingEvents() => _pendingEvents.Clear();
	}
}
