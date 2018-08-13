using System;
using System.Collections.Generic;
using Crispin.Conditions;
using Crispin.Events;

namespace Crispin.Views
{
	public class StatisticView
	{
		public DateTime? LastQueried { get; set; }
		public DateTime? LastActive { get; set; }
		public DateTime? LastInactive { get; set; }

		public int ActivePercentage { get; set; }
		public int TotalQueries { get; set; }
		public int ActiveQueries { get; set; }

		public Dictionary<DateTime, int> QueryGraph { get; set; }
		public Dictionary<DateTime, int> ActiveGraph { get; set; }
		public Dictionary<DateTime, int> InactiveGraph { get; set; }

		public Dictionary<ConditionID, ConditionStatisticView> Conditions { get; set; }

		public StatisticView()
		{
			QueryGraph = new Dictionary<DateTime, int>();
			ActiveGraph = new Dictionary<DateTime, int>();
			InactiveGraph = new Dictionary<DateTime, int>();
			Conditions = new Dictionary<ConditionID, ConditionStatisticView>();
		}

		public void Apply(StatisticReceived @event)
		{
			UpdateCounts(@event);
			UpdateTimestamps(@event);
			UpdateGraphs(@event);
			UpdateConditions(@event);
		}

		private void UpdateCounts(StatisticReceived @event)
		{
			TotalQueries++;

			if (@event.Active)
				ActiveQueries++;

			ActivePercentage = (int)((100M / TotalQueries) * ActiveQueries);
		}

		private void UpdateTimestamps(StatisticReceived @event)
		{
			LastQueried = Latest(LastQueried, @event.Timestamp);

			if (@event.Active)
				LastActive = Latest(LastActive, @event.Timestamp);
			else
				LastInactive = Latest(LastInactive, @event.Timestamp);
		}

		private void UpdateGraphs(StatisticReceived @event)
		{
			var groupTime = Truncate(@event.Timestamp);

			QueryGraph.TryGetValue(groupTime, out var queryCount);
			QueryGraph[groupTime] = ++queryCount;

			if (@event.Active)
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

		private void UpdateConditions(StatisticReceived @event)
		{
			foreach (var state in @event.ConditionStates)
			{
				if (Conditions.TryGetValue(state.Key, out var view) == false)
					Conditions[state.Key] = view = new ConditionStatisticView();

				view.Apply(@event, state.Value);
			}
		}

		private static DateTime Latest(DateTime? one, DateTime two) => one.HasValue && one.Value >= two ? one.Value : two;
		private static DateTime Truncate(DateTime value) => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
	}
}
