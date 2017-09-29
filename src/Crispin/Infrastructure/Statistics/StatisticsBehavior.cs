using System.Threading.Tasks;
using MediatR;

namespace Crispin.Infrastructure.Statistics
{
	public class StatisticsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IStatisticsWriter _writer;

		public StatisticsBehavior(IStatisticsWriter writer)
		{
			_writer = writer;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
			if (request is IStatisticGenerator requestStatistic)
				await requestStatistic.Write(_writer);

			var response = await next();

			if (response is IStatisticGenerator responseStatistic)
				await responseStatistic.Write(_writer);

			return response;
		}
	}
}
