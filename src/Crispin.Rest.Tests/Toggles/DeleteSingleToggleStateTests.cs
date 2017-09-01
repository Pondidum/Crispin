using System;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Handlers.UpdateState;
using Crispin.Projections;
using Crispin.Rest.Toggles;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class DeleteSingleToggleStateTests : TogglesControllerTests
	{
		private readonly Guid _toggleID;
		private UpdateToggleStateRequest _request;

		public DeleteSingleToggleStateTests()
		{
			_toggleID = Guid.NewGuid();

			Mediator
				.When(m => m.Send(Arg.Any<UpdateToggleStateRequest>()))
				.Do(ci => { _request = ci.Arg<UpdateToggleStateRequest>(); });
		}

		[Fact]
		public async Task When_deleteing_state_for_a_non_existing_toggle()
		{
			Mediator
				.Send(Arg.Any<UpdateToggleStateRequest>())
				.Returns(new UpdateToggleStateResponse());

			var response = (JsonResult)await Controller.DeleteState(_toggleID, new DeleteStateModel());

			response.Value.ShouldBe(null);
		}

		[Fact]
		public async Task When_deleting_state_for_an_existing_toggle()
		{
			Mediator
				.Send(Arg.Any<UpdateToggleStateRequest>())
				.Returns(new UpdateToggleStateResponse { State = new StateView() });

			var response = (JsonResult)await Controller.DeleteState(_toggleID, new DeleteStateModel
			{
				Groups = new[] { "group-1" },
				Users = new[] { "user-1" }
			});

			_request.ShouldSatisfyAllConditions(
				() => _request.ToggleID.ShouldBe(ToggleID.Parse(_toggleID)),
				() => _request.Anonymous.ShouldBe(null),
				() => _request.Users.Single().Key.ShouldBe(UserID.Parse("user-1")),
				() => _request.Users.Single().Value.ShouldBe(null),
				() => _request.Groups.Single().Key.ShouldBe(GroupID.Parse("group-1")),
				() => _request.Groups.Single().Value.ShouldBe(null),
				() => response.Value.ShouldBeOfType<StateView>()
			);
		}
	}
}
