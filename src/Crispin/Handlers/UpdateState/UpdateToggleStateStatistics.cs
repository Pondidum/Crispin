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

			if (request.Default.HasValue)
				await writer.WriteCount(new ToggleDefaultStateChange(
					response.ToggleID,
					request.Default.Value));

			foreach (var user in request.Users)
				await writer.WriteCount(new ToggleUserStateChange(
					response.ToggleID,
					user.Key,
					user.Value
				));

			foreach (var group in request.Groups)
				await writer.WriteCount(new ToggleGroupStateChange(
					response.ToggleID,
					group.Key,
					group.Value
				));
		}
	}

	public struct ToggleDefaultStateChange : IStat
	{
		public ToggleID ToggleID { get; }
		public States State { get; }

		public ToggleDefaultStateChange(ToggleID toggleID, States state)
		{
			ToggleID = toggleID;
			State = state;
		}

		public override string ToString()
		{
			return $"Toggle '{ToggleID}' default state changed to {State}";
		}
	}

	public struct ToggleUserStateChange : IStat
	{
		public ToggleID ToggleID { get; }
		public UserID UserID { get; }
		public States? State { get; }

		public ToggleUserStateChange(ToggleID toggleID, UserID userID, States? state)
		{
			ToggleID = toggleID;
			UserID = userID;
			State = state;
		}

		public override string ToString()
		{
			return $"Toggle '{ToggleID}' state for user '{UserID}' changed to {State.Render()}";
		}
	}

	public struct ToggleGroupStateChange : IStat
	{
		public ToggleID ToggleID { get; }
		public GroupID GroupID { get; }
		public States? State { get; }

		public ToggleGroupStateChange(ToggleID toggleID, GroupID groupID, States? state)
		{
			ToggleID = toggleID;
			GroupID = groupID;
			State = state;
		}

		public override string ToString()
		{
			return $"Toggle '{ToggleID}' state for group '{GroupID}' changed to {State.Render()}";
		}
	}
}
