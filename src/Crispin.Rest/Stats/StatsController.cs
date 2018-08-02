using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Conditions;
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

		public async Task<IActionResult> Post([FromBody] ToggleStatisticsPostRequest[] request)
		{
			return new JsonResult(new { Count = request.Length });
		}
	}

	public class ToggleStatisticsPostRequest
	{
		public ToggleID ToggleID { get; set; }
		public UserID User { get; set; }
		public DateTime Timestamp { get; set; }
		public bool Active { get; set; }
		
		public Dictionary<ConditionID, bool> ConditionStates { get; set; } 
	}
}
