using System.Collections.Generic;
using MediatR;

namespace Crispin.Handlers.WriteStatistics
{
	public class WriteStatisticsRequest : IRequest<WriteStatisticsResponse>
	{
		public WriteStatisticsRequest(Statistic[] request)
		{
			Statistics = request;
		}

		public IEnumerable<Statistic> Statistics { get; set; }
	}
}
