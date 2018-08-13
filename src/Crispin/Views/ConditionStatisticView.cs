using System;
using System.Collections.Generic;
using Crispin.Events;

namespace Crispin.Views
{
	public class ConditionStatisticView
	{
		public DateTime? LastActive { get; set; }
		public DateTime? LastInactive { get; set; }

		public Dictionary<DateTime, int> QueryGraph { get; set; }
		public Dictionary<DateTime, int> ActiveGraph { get; set; }
		public Dictionary<DateTime, int> InactiveGraph { get; set; }

		public ConditionStatisticView()
		{
			QueryGraph = new Dictionary<DateTime, int>();
			ActiveGraph = new Dictionary<DateTime, int>();
			InactiveGraph = new Dictionary<DateTime, int>();
		}

		public void Apply(StatisticReceived @event, bool active)
		{
			if (active)
				LastActive = Latest(LastActive, @event.Timestamp);
			else
				LastInactive = Latest(LastInactive, @event.Timestamp);

			var groupTime = Truncate(@event.Timestamp);

			QueryGraph.TryGetValue(groupTime, out var queryCount);
			QueryGraph[groupTime] = ++queryCount;

			if (active)
			{
				ActiveGraph.TryGetValue(groupTime, out var activeCount);
				ActiveGraph[groupTime] = ++activeCount;
			}
			else
			{
				InactiveGraph.TryGetValue(groupTime, out var inactiveCount);
				InactiveGraph[groupTime] = ++inactiveCount;
			}
		}


		private static DateTime Latest(DateTime? one, DateTime two) => one.HasValue && one.Value >= two ? one.Value : two;
		private static DateTime Truncate(DateTime value) => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
	}
}
