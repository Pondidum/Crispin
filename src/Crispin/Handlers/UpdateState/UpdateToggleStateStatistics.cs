using System;
using System.Threading.Tasks;
using Crispin.Infrastructure.Statistics;

namespace Crispin.Handlers.UpdateState
{
	public class UpdateToggleStateStatistics : IStatisticGenerator<UpdateToggleStateRequest, UpdateToggleStateResponse>
	{
		public async Task Write(IStatisticsWriter writer, UpdateToggleStateRequest request, UpdateToggleStateResponse response)
		{
			if (response.State == null)
				return;

			var state = new Func<States?, string>(s => s.HasValue
				? s.Value.ToString()
				: "unset");

			if (request.Anonymous.HasValue)
			{
				await writer.WriteCount(
					"toggle.{toggleID}.state.anonymous.{state}",
					response.ToggleID,
					state(request.Anonymous));
			}

			foreach (var user in request.Users)
			{
				await writer.WriteCount(
					"toggle.{toggleID}.state.users.{userID}.{state}",
					response.ToggleID,
					user.Key,
					state(user.Value));
			}

			foreach (var group in request.Groups)
			{
				await writer.WriteCount(
					"toggle.{toggleID}.state.groups.{groupID}.{state}",
					response.ToggleID,
					group.Key,
					state(group.Value));
			}
		}
	}
}
