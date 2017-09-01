using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers.UpdateState;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class UpdateToggleStateHandlerTests
	{
		private readonly UpdateToggleStateHandler _handler;
		private readonly ToggleID _toggleID;
		private readonly Dictionary<ToggleID, List<Event>> _events;

		public UpdateToggleStateHandlerTests()
		{
			_events = new Dictionary<ToggleID, List<Event>>();

			var storage = new InMemoryStorage(_events);
			storage.RegisterBuilder(events => Toggle.LoadFrom(() => "", events));
			storage.RegisterProjection(new AllToggles());

			_handler = new UpdateToggleStateHandler(storage);
			var toggle = Toggle.CreateNew(() => "", "name", "desc");

			using (var session = storage.BeginSession())
				session.Save(toggle);

			_toggleID = toggle.ID;
		}

		private IEnumerable<Type> EventTypes() => _events[_toggleID].Select(e => e.GetType());
		private TEvent Event<TEvent>() => _events[_toggleID].OfType<TEvent>().Single();

		[Fact]
		public void When_the_toggle_doesnt_exist()
		{
			var toggleID = ToggleID.CreateNew();

			Should.Throw<KeyNotFoundException>(
				() => _handler.Handle(new UpdateToggleStateRequest(toggleID))
			);

			_events.ShouldNotContainKey(toggleID);
		}

		[Fact]
		public async Task When_updating_a_toggle_with_no_current_state_for_anonymous()
		{
			var response = await _handler.Handle(new UpdateToggleStateRequest(_toggleID)
			{
				Anonymous = States.On
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(ToggleSwitchedOnForAnonymous)
			});
		}


		[Fact]
		public async Task When_switching_on_for_a_user()
		{
			var userID = UserID.Parse("user-1");
			var response = await _handler.Handle(new UpdateToggleStateRequest(_toggleID)
			{
				Users = { { userID, States.On } }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(ToggleSwitchedOnForUser)
			});

			Event<ToggleSwitchedOnForUser>().User.ShouldBe(userID);
		}

		[Fact]
		public async Task When_switching_off_for_a_user()
		{
			var userID = UserID.Parse("user-1");
			var response = await _handler.Handle(new UpdateToggleStateRequest(_toggleID)
			{
				Users = { { userID, States.Off } }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(ToggleSwitchedOffForUser)
			});

			Event<ToggleSwitchedOffForUser>().User.ShouldBe(userID);
		}
		
		[Fact]
		public async Task When_switching_on_for_a_group()
		{
			var groupID = GroupID.Parse("group-1");
			var response = await _handler.Handle(new UpdateToggleStateRequest(_toggleID)
			{
				Groups = { { groupID, States.On } }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(ToggleSwitchedOnForGroup)
			});

			Event<ToggleSwitchedOnForGroup>().Group.ShouldBe(groupID);
		}

		[Fact]
		public async Task When_switching_off_for_a_group()
		{
			var groupID = GroupID.Parse("group-1");
			var response = await _handler.Handle(new UpdateToggleStateRequest(_toggleID)
			{
				Groups = { { groupID, States.Off } }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(ToggleSwitchedOffForGroup)
			});

			Event<ToggleSwitchedOffForGroup>().Group.ShouldBe(groupID);
		}
	}
}
