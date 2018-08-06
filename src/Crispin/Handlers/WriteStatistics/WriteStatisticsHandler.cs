using System;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.WriteStatistics
{
	public class WriteStatisticsHandler : IAsyncRequestHandler<WriteStatisticsRequest, WriteStatisticsResponse>
	{
		private readonly IStorageSession _session;

		public WriteStatisticsHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<WriteStatisticsResponse> Handle(WriteStatisticsRequest message)
		{
			await _session.Save(new EventAdaptor<Statistic>(message.Statistics, CreateEvent));

			return new WriteStatisticsResponse();
		}

		private static IEvent CreateEvent(Statistic stat) => new Event<StatisticReceived>
		{
			AggregateID = stat.ToggleID,
			TimeStamp = DateTime.Now,
			Data = new StatisticReceived(stat.User, stat.Timestamp, stat.Active, stat.ConditionStates)
		};
	}
}
