using System;
using System.Collections.Generic;
using Crispin.Infrastructure;

namespace Crispin.Projections
{
	public class LoggingProjection : Projection<LoggingMemento>
	{
		public List<LogEntry> Messages { get; }

		public LoggingProjection()
		{
			Messages = new List<LogEntry>();
			RegisterAll(@event => Messages.Add(new LogEntry
			{
				TimeStamp = @event.TimeStamp,
				Message = @event.ToString()
			}));
		}

		public class LogEntry
		{
			public DateTime TimeStamp { get; set; }
			public string Message { get; set; }
		}

		protected override LoggingMemento CreateMemento()
		{
			return new LoggingMemento(Messages);
		}

		protected override void ApplyMemento(LoggingMemento memento)
		{
			Messages.AddRange(memento);
		}
	}

	public class LoggingMemento : List<LoggingProjection.LogEntry>
	{
		public LoggingMemento()
		{
		}

		public LoggingMemento(IEnumerable<LoggingProjection.LogEntry> others) : base(others)
		{
		}
	}
}
