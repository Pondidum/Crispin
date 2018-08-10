using Crispin.Events;

namespace Crispin.Views
{
	public class StatisticView
	{
		public int ActivePercentage { get; set; }
		public int TotalQueries { get; set; }
		public int ActiveQueries { get; set; }

		public void Apply(StatisticReceived @event)
		{
			TotalQueries++;

			if (@event.Active)
				ActiveQueries++;

			ActivePercentage = (int)((100M / TotalQueries) * ActiveQueries);
		}
	}
}

