using System;
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
		}

		private static DateTime Latest(DateTime one, DateTime two) => one >= two ? one : two;
	}
}
