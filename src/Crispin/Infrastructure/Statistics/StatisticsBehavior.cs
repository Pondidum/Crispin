using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace Crispin.Infrastructure.Statistics
{
	public class StatisticsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IStatisticsWriter _writer;
		private readonly IEnumerable<IStatisticGenerator<TRequest, TResponse>> _generators;

		public StatisticsBehavior(IStatisticsWriter writer, IEnumerable<IStatisticGenerator<TRequest, TResponse>> generators)
		{
			_writer = writer;
			_generators = generators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
		{
			var response = await next();

			foreach (var generator in _generators)
				await generator.Write(_writer, request, response);

			return response;
		}
	}
}
