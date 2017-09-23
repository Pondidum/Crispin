using System.Threading.Tasks;
using Crispin.Handlers.GetSingle;
using Crispin.Handlers.UpdateState;
using Crispin.Projections;
using Crispin.Rest.Toggles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Toggles
{
	public class ToggleStateControllerTests
	{
		private UpdateToggleStateRequest _request;

		private readonly ToggleLocator _locator;
		private readonly ToggleStateController _controller;
		private readonly IMediator _mediator;
		private readonly ToggleLocator _invalidLocator;

		public ToggleStateControllerTests()
		{
			var response = new UpdateToggleStateResponse
			{
				State = new StateView()
			};

			_mediator = Substitute.For<IMediator>();
			_controller = new ToggleStateController(_mediator);
			_locator = ToggleLocator.Create(ToggleID.CreateNew());
			_invalidLocator = ToggleLocator.Create(ToggleID.CreateNew());

			_mediator
				.Send(Arg.Any<GetToggleRequest>())
				.Returns(new GetToggleResponse());

			_mediator
				.Send(Arg.Is<GetToggleRequest>(r => r.Locator == _locator))
				.Returns(new GetToggleResponse { Toggle = new ToggleView() });

			_mediator
				.Send(Arg.Any<UpdateToggleStateRequest>())
				.Returns(new UpdateToggleStateResponse());

			_mediator
				.Send(Arg.Is<UpdateToggleStateRequest>(req => req.Locator == _locator))
				.Returns(response)
				.AndDoes(ci => _request = ci.Arg<UpdateToggleStateRequest>());
		}

		[Fact]
		public async Task When_fetching_state_by_id()
		{
			var response = (JsonResult)await _controller.GetState(_locator);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == _locator));
			response.Value.ShouldBeOfType<StateView>();
		}

		[Fact]
		public async Task When_fetching_state_by_id_which_doesnt_exist()
		{
			var response = (JsonResult)await _controller.GetState(_invalidLocator);

			await _mediator.Received().Send(Arg.Is<GetToggleRequest>(req => req.Locator == _invalidLocator));
			response.Value.ShouldBeNull();
		}

		[Fact]
		public async Task When_putting_default_state_and_the_toggle_doesnt_exist()
		{
			var response = (JsonResult)await _controller.PutStateDefault(
				_invalidLocator,
				new StatePutRequest { State = States.On });

			response.Value.ShouldBeNull();
		}

		[Fact]
		public async Task When_putting_default_state()
		{
			var response = (JsonResult)await _controller.PutStateDefault(
				_locator,
				new StatePutRequest { State = States.On });

			_request.ShouldSatisfyAllConditions(
				() => _request.Anonymous.ShouldBe(States.On),
				() => _request.Users.ShouldBeEmpty(),
				() => _request.Groups.ShouldBeEmpty(),
				() => response.Value.ShouldBeOfType<StateView>()
			);
		}

		[Fact]
		public async Task When_putting_user_state_and_the_toggle_doesnt_exist()
		{
			var response = (JsonResult)await _controller.PutStateUser(
				_invalidLocator,
				UserID.Parse("wat"), 
				new StatePutRequest { State = States.On });

			response.Value.ShouldBeNull();
		}

		[Fact]
		public async Task When_putting_user_state()
		{
			var userid = UserID.Parse("wat");
			var response = (JsonResult)await _controller.PutStateUser(
				_locator,
				userid,
				new StatePutRequest { State = States.On });

			_request.ShouldSatisfyAllConditions(
				() => _request.Anonymous.ShouldBeNull(),
				() => _request.Users.ShouldContainKeyAndValue(userid, States.On),
				() => _request.Users.ShouldHaveSingleItem(),
				() => _request.Groups.ShouldBeEmpty(),
				() => response.Value.ShouldBeOfType<StateView>()
			);
		}

		[Fact]
		public async Task When_putting_group_state_and_the_toggle_doesnt_exist()
		{
			var response = (JsonResult)await _controller.PutStateGroup(
				_invalidLocator,
				GroupID.Parse("wat"),
				new StatePutRequest { State = States.On });

			response.Value.ShouldBeNull();
		}

		[Fact]
		public async Task When_putting_group_state()
		{
			var groupid = GroupID.Parse("wat");
			var response = (JsonResult)await _controller.PutStateGroup(
				_locator,
				groupid,
				new StatePutRequest { State = States.On });

			_request.ShouldSatisfyAllConditions(
				() => _request.Anonymous.ShouldBeNull(),
				() => _request.Users.ShouldBeEmpty(),
				() => _request.Groups.ShouldContainKeyAndValue(groupid, States.On),
				() => _request.Groups.ShouldHaveSingleItem(),
				() => response.Value.ShouldBeOfType<StateView>()
			);
		}

		[Fact]
		public async Task When_removing_user_state()
		{
			var userid = UserID.Parse("wat");
			var response = (JsonResult)await _controller.DeleteStateUser(
				_locator,
				userid);

			_request.ShouldSatisfyAllConditions(
				() => _request.Anonymous.ShouldBeNull(),
				() => _request.Users.ShouldContainKeyAndValue(userid, null),
				() => _request.Users.ShouldHaveSingleItem(),
				() => _request.Groups.ShouldBeEmpty(),
				() => response.Value.ShouldBeOfType<StateView>()
			);
		}

		[Fact]
		public async Task When_removing_group_state()
		{
			var groupid = GroupID.Parse("wat");
			var response = (JsonResult)await _controller.DeleteStateGroup(
				_locator,
				groupid);

			_request.ShouldSatisfyAllConditions(
				() => _request.Anonymous.ShouldBeNull(),
				() => _request.Users.ShouldBeEmpty(),
				() => _request.Groups.ShouldContainKeyAndValue(groupid, null),
				() => _request.Groups.ShouldHaveSingleItem(),
				() => response.Value.ShouldBeOfType<StateView>()
			);
		}
	}
}
