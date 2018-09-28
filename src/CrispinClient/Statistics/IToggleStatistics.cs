using System;

namespace CrispinClient.Statistics
{
	public interface IToggleStatistics
	{
		IToggleReporter CreateReporter(Guid toggleID);
	}
}
