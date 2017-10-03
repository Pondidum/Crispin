using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers.UpdateState;
using Crispin.Infrastructure.Statistics;
using Crispin.Projections;
using NSubstitute;
using Xunit;

namespace Crispin.Tests.Handlers.UpdateState
{
	public class UpdateToggleStateStatisticsTests
	{
		private readonly UpdateToggleStateStatistics _stats;
		private readonly UpdateToggleStateResponse _response;

		public UpdateToggleStateStatisticsTests()
		{
			_stats = new UpdateToggleStateStatistics();
			_response = new UpdateToggleStateResponse
			{
				ToggleID = ToggleID.CreateNew(),
				State = new StateView()
			};
		}

		[Theory]
		[InlineData(null, "")]
		[InlineData(States.Off, "off")]
		[InlineData(States.On, "on")]
		public async Task When_writing_an_anonymous_event(States? state, string expected)
		{
			var request = new UpdateToggleStateRequest(EditorID.Parse("me"), null)
			{
				Anonymous = state
			};

			var writer = Substitute.For<IStatisticsWriter>();
			await _stats.Write(writer, request, _response);

			if (state.HasValue)
				await writer.Received().WriteCount(
					"toggle.{toggleID}.state.anonymous.{state}",
					_response.ToggleID,
					expected);
			else
				await writer.DidNotReceive().WriteCount(
					Arg.Any<string>(),
					Arg.Any<object[]>());
		}

		[Theory]
		[InlineData(null, "unset")]
		[InlineData(States.Off, "off")]
		[InlineData(States.On, "on")]
		public async Task When_writing_a_user_event(States? state, string expected)
		{
			var userID = UserID.Parse("wat");
			var request = new UpdateToggleStateRequest(EditorID.Parse("me"), null)
			{
				Users = new Dictionary<UserID, States?> { { userID, state } }
			};

			var writer = Substitute.For<IStatisticsWriter>();
			await _stats.Write(writer, request, _response);

			await writer.Received().WriteCount(
				"toggle.{toggleID}.state.users.{userID}.{state}",
				_response.ToggleID,
				userID,
				expected);
		}

		[Theory]
		[InlineData(null, "unset")]
		[InlineData(States.Off, "off")]
		[InlineData(States.On, "on")]
		public async Task When_writing_a_groups_event(States? state, string expected)
		{
			var groupID = GroupID.Parse("wat");
			var request = new UpdateToggleStateRequest(EditorID.Parse("me"), null)
			{
				Groups = new Dictionary<GroupID, States?> { { groupID, state } }
			};

			var writer = Substitute.For<IStatisticsWriter>();
			await _stats.Write(writer, request, _response);

			await writer.Received().WriteCount(
				"toggle.{toggleID}.state.groups.{groupID}.{state}",
				_response.ToggleID,
				groupID,
				expected);
		}
	}
}
