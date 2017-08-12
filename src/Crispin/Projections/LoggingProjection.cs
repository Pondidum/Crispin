using System;
using System.Collections.Generic;
using Crispin.Infrastructure;

namespace Crispin.Projections
{
	public class LoggingProjection : Projection
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
	}
}
