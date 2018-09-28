using System;
using CrispinClient.Conditions;

namespace CrispinClient
{
	public interface IToggleReporter : IDisposable
	{
		void Report(Condition condition, bool isActive);
	}
}
