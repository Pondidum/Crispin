using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers.UpdateState;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class UpdateToggleStateHandlerTests
	{
		private readonly UpdateToggleStateHandler _handler;
		private readonly IStorageSession _session;
		private readonly List<object> _events;
		private readonly ToggleID _toggleID;

		public UpdateToggleStateHandlerTests()
		{
			_events = new List<object>();
			var storage = Substitute.For<IStorage>();

			_session = Substitute.For<IStorageSession>();
			_session
				.When(x => x.Save(Arg.Any<AggregateRoot>()))
				.Do(ci => _events.AddRange(((IEvented)ci.Arg<AggregateRoot>()).GetPendingEvents()));
			_handler = new UpdateToggleStateHandler(storage);

			storage.BeginSession().Returns(_session);

			_toggleID = ToggleID.CreateNew();
			var toggle = Toggle.LoadFrom(() => "", new[]
			{
				new ToggleCreated(_toggleID, "name", "desc"),
			});

			_session.LoadAggregate<Toggle>(toggle.ID).Returns(toggle);
		}

		[Fact]
		public async Task When_the_toggle_doesnt_exist()
		{
			var response = await _handler.Handle(new UpdateToggleStateRequest(ToggleID.CreateNew(), null, null, null));

			_events.ShouldBeEmpty();
		}

		[Fact]
		public async Task When_updating_a_toggle_with_no_current_state()
		{
			var response = await _handler.Handle(new UpdateToggleStateRequest(
				_toggleID,
				anonymous: true,
				groups: null,
				users: null
			));

			var switchEvent = (ToggleSwitchedOn)_events.ShouldHaveSingleItem();

			switchEvent.ShouldSatisfyAllConditions(
				() => switchEvent.User.ShouldBeNullOrWhiteSpace(),
				() => switchEvent.Group.ShouldBeNullOrWhiteSpace()
			);
		}
	}
}
