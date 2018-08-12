using System;
using System.Collections.Generic;
using Crispin.Events;

namespace Crispin.Views
{
	public class StatisticView
	{
		public DateTime LastQueried { get; set; }
		public DateTime LastActive { get; set; }
		public DateTime LastInactive { get; set; }

		public int ActivePercentage { get; set; }
		public int TotalQueries { get; set; }
		public int ActiveQueries { get; set; }

		public Dictionary<DateTime, int> ActiveGraph { get; set; }
		public Dictionary<DateTime, int> QueryGraph { get; set; }

		public StatisticView()
		{
			ActiveGraph = new Dictionary<DateTime, int>();
			QueryGraph = new Dictionary<DateTime, int>();
		}

		public void Apply(StatisticReceived @event)
		{
			TotalQueries++;

			if (@event.Active)
				ActiveQueries++;

			ActivePercentage = (int)((100M / TotalQueries) * ActiveQueries);

			LastQueried = Latest(LastQueried, @event.Timestamp);

			if (@event.Active)
				LastActive = Latest(LastActive, @event.Timestamp);
			else
				LastInactive = Latest(LastInactive, @event.Timestamp);

			var groupTime = Truncate(@event.Timestamp);

			QueryGraph.TryGetValue(groupTime, out var queryCount);
			QueryGraph[groupTime] = ++queryCount;

			if (@event.Active)
			{
				ActiveGraph.TryGetValue(groupTime, out var activeCount);
				ActiveGraph[groupTime] = ++activeCount;
			}
		}

		private static DateTime Latest(DateTime one, DateTime two) => one >= two ? one : two;
		private static DateTime Truncate(DateTime value) => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
	}
}
