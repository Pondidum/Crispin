using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrispinClient.Infrastructure;

namespace CrispinClient.Fetching
{
	public class PeriodicFetcher : IToggleFetcher, IDisposable
	{
		private Dictionary<Guid, Toggle> _toggles;
		private readonly CancellationTokenSource _source;
		private readonly Task _backgroundFetch;

		public PeriodicFetcher(ICrispinClient client, TimeSpan frequency, ITimeControl timeControl = null)
		{
			timeControl = timeControl ?? new RealTimeControl();

			_toggles = new Dictionary<Guid, Toggle>();
			_source = new CancellationTokenSource();
			_backgroundFetch = Task.Run(async () =>
			{
				while (_source.IsCancellationRequested == false)
				{
					await timeControl.Delay(frequency, _source.Token);
					_toggles = client.GetAllToggles().ToDictionary(t => t.ID);

				}
			}, _source.Token);

			_toggles = client.GetAllToggles().ToDictionary(t => t.ID);
		}

		public IReadOnlyDictionary<Guid, Toggle> GetAllToggles() => _toggles;

		public void Dispose()
		{
			_source.Cancel();
			_source.Dispose();
			_backgroundFetch.Dispose();
		}
	}
}
