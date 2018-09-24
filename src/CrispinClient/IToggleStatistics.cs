using System;

namespace CrispinClient
{
	public interface IToggleStatistics
	{
		IToggleReporter CreateReporter(Guid toggleID);
	}
}
