using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrispinClient.Infrastructure;

namespace CrispinClient.Statistics.Writers
{
	public class BatchingWriter : IStatisticsWriter, IDisposable
	{
		private readonly ICrispinClient _client;
		private readonly int _batchSize;

		private readonly ConcurrentQueue<Statistic> _pending;
		private readonly Func<Task> _cancel;

		public BatchingWriter(ICrispinClient client, int batchSize = 5, ITimeControl time = null, TimeSpan interval = default(TimeSpan))
		{
			time = time ?? new RealTimeControl();
			interval = interval != TimeSpan.Zero
				? interval
				: TimeSpan.FromSeconds(5);

			_client = client;
			_batchSize = batchSize;
			_pending = new ConcurrentQueue<Statistic>();

			_cancel = time.Every(interval, Send);
		}

		public void Write(Statistic statistic)
		{
			_pending.Enqueue(statistic);

			if (_pending.Count >= _batchSize)
				Send();
		}

		private void Send()
		{
			var items = Enumerable
				.Range(0, _batchSize).Select(i =>
				{
					var success = _pending.TryDequeue(out var item);
					return new { success, item };
				})
				.Where(x => x.success)
				.Select(x => x.item)
				.ToArray();

			_client.SendStatistics(items);
		}

		public void Dispose()
		{
			_cancel().GetAwaiter().GetResult();
		}
	}
}
