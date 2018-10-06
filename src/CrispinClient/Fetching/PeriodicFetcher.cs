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
		private readonly TaskCompletionSource<bool> _initialLoadDone;

		public PeriodicFetcher(ICrispinClient client, TimeSpan frequency, ITimeControl timeControl = null)
		{
			timeControl = timeControl ?? new RealTimeControl();

			_toggles = new Dictionary<Guid, Toggle>();
			_source = new CancellationTokenSource();
			_initialLoadDone = new TaskCompletionSource<bool>();

			_backgroundFetch = Task.Run(async () =>
			{
				await SafeReadToggles(client);
				_initialLoadDone.SetResult(true);

				while (_source.IsCancellationRequested == false)
				{
					await timeControl.Delay(frequency, _source.Token);
					await SafeReadToggles(client);
				}
			}, _source.Token);
		}

		private async Task SafeReadToggles(ICrispinClient client)
		{
			try
			{
				_toggles = (await client.GetAllToggles()).ToDictionary(t => t.ID);
			}
			catch (Exception)
			{
			}
		}

		public async Task<IReadOnlyDictionary<Guid, Toggle>> GetAllToggles()
		{
			await _initialLoadDone.Task;
			return _toggles;
		}

		public void Dispose()
		{
			_source.Cancel();
			_source.Dispose();
			_backgroundFetch.GetAwaiter().GetResult();
			_backgroundFetch.Dispose();
		}
	}
}
