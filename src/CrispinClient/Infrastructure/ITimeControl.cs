using System;
using System.Threading;
using System.Threading.Tasks;

namespace CrispinClient.Infrastructure
{
	public interface ITimeControl
	{
		Task Delay(TimeSpan time, CancellationToken cancellationToken);
	}
}