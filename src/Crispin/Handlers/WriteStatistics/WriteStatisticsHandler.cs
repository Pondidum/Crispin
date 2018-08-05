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
		private static readonly Guid StatisticsID = Guid.Parse("2914B058-8AA5-42AF-9379-7671B4760AA1");

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
			AggregateID = StatisticsID,
			TimeStamp = DateTime.Now,
			Data = new StatisticReceived(stat.ToggleID, stat.User, stat.Timestamp, stat.Active, stat.ConditionStates)
		};
	}
}
