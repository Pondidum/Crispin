using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
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
			//AggregateID = Guid.NewGuid(),
			TimeStamp = DateTime.Now,
			Data = new StatisticReceived(stat.ToggleID, stat.User, stat.Timestamp, stat.Active, stat.ConditionStates)
		};
	}

	public class WriteStatisticsResponse
	{
	}

	public class WriteStatisticsRequest : IRequest<WriteStatisticsResponse>
	{
		public WriteStatisticsRequest(Statistic[] request)
		{
			Statistics = request;
		}

		public IEnumerable<Statistic> Statistics { get; set; }
	}

	public class Statistic
	{
		public ToggleID ToggleID { get; set; }
		public UserID User { get; set; }
		public DateTime Timestamp { get; set; }
		public bool Active { get; set; }

		public Dictionary<ConditionID, bool> ConditionStates { get; set; }
	}


	public class EventAdaptor<T> : IEvented
	{
		private readonly IEnumerable<T> _collection;
		private readonly Func<T, IEvent> _createEvent;
		private bool _cleared;

		public EventAdaptor(IEnumerable<T> collection, Func<T, IEvent> createEvent)
		{
			_collection = collection;
			_createEvent = createEvent;
			_cleared = false;
		}

		public IEnumerable<IEvent> GetPendingEvents() => _cleared
			? Enumerable.Empty<IEvent>()
			: _collection.Select(_createEvent);

		public void ClearPendingEvents() => _cleared = true;
	}
}
