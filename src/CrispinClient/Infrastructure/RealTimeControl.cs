using System;
using System.Threading;
using System.Threading.Tasks;

namespace CrispinClient.Infrastructure
{
	public class RealTimeControl : ITimeControl
	{
		public Task Delay(TimeSpan time, CancellationToken cancellationToken) => Task.Delay(time, cancellationToken);
	}
}
