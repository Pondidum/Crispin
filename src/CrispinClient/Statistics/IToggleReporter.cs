using System;
using CrispinClient.Conditions;

namespace CrispinClient.Statistics
{
	public interface IToggleReporter : IDisposable
	{
		void Report(Condition condition, bool isActive);
		void Report(Toggle condition, bool isActive);
	}
}
