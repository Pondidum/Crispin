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

		[Fact]
		public void When_the_toggle_doesnt_exist()
		{
			var toggleID = ToggleID.CreateNew();

			Should.Throw<KeyNotFoundException>(
				() => _handler.Handle(new UpdateToggleStateRequest(toggleID, null, null, null))
			);

			_events.ShouldNotContainKey(toggleID);
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

			_events[_toggleID].Select(e => e.GetType()).ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(ToggleSwitchedOnForAnonymous)
			});
		}
	}
}
