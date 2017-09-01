﻿using System;
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
	public class PostSingleToggleStateTests : TogglesControllerTests
	{
		private readonly Guid _toggleID;
		private UpdateToggleStateRequest _request;

		public PostSingleToggleStateTests()
		{
			_toggleID = Guid.NewGuid();

			Mediator
				.When(m => m.Send(Arg.Any<UpdateToggleStateRequest>()))
				.Do(ci => { _request = ci.Arg<UpdateToggleStateRequest>(); });
		}

		[Fact]
		public async Task When_setting_state_for_a_non_existing_toggle()
		{
			Mediator
				.Send(Arg.Any<UpdateToggleStateRequest>())
				.Returns(new UpdateToggleStateResponse());

			var response = (JsonResult) await Controller.PostState(_toggleID, new UpdateStateModel());

			response.Value.ShouldBe(null);
		}

		[Fact]
		public async Task When_setting_state_for_an_existing_toggle()
		{
			Mediator
				.Send(Arg.Any<UpdateToggleStateRequest>())
				.Returns(new UpdateToggleStateResponse { State = new StateView() });

			var model = new UpdateStateModel
			{
				Anonymous = true,
				Groups = { { "group-1", true } },
				Users = { { "user-1", false } }
			};

			var response = (JsonResult)await Controller.PostState(_toggleID, model);

			_request.ShouldSatisfyAllConditions(
				() => _request.ToggleID.ShouldBe(ToggleID.Parse(_toggleID)),
				() => _request.Anonymous.ShouldBe(States.On),
				() => _request.Users.Single().Key.ShouldBe(UserID.Parse("user-1")),
				() => _request.Users.Single().Value.ShouldBe(States.Off),
				() => _request.Groups.Single().Key.ShouldBe(GroupID.Parse("group-1")),
				() => _request.Groups.Single().Value.ShouldBe(States.On),
				() => response.Value.ShouldBeOfType<StateView>()
			);
		}
	}
}
