using System.Threading.Tasks;
using Crispin.Handlers.WriteStatistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crispin.Rest.Stats
{
	public class StatsController
	{
		private readonly IMediator _mediator;

		public StatsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> Post([FromBody] Statistic[] request)
		{
			await _mediator.Send(new WriteStatisticsRequest(request));

			return new OkResult();
		}
	}
}
