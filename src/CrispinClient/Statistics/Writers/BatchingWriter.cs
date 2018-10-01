using System.Collections.Generic;

namespace CrispinClient.Statistics.Writers
{
	public class BatchingWriter : IStatisticsWriter
	{
		private readonly ICrispinClient _client;
		private readonly List<Statistic> _pending;
		private readonly int _batchSize;

		public BatchingWriter(ICrispinClient client, int batchSize = 5)
		{
			_client = client;
			_batchSize = batchSize;
			_pending = new List<Statistic>(batchSize);
		}

		public void Write(Statistic statistic)
		{
			_pending.Add(statistic);

			if (_pending.Count >= _batchSize)
				Send();
		}

		private void Send()
		{
			_client.SendStatistics(_pending);
			_pending.Clear();
		}
	}
}
