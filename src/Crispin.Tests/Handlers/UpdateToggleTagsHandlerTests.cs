using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers.UpdateTags;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class UpdateToggleTagsHandlerTests
	{
		private readonly Dictionary<ToggleID, List<Event>> _events;
		private readonly UpdateToggleTagsHandler _handler;
		private readonly ToggleID _toggleID;

		public UpdateToggleTagsHandlerTests()
		{
			_events = new Dictionary<ToggleID, List<Event>>();

			var storage = new InMemoryStorage(_events);
			storage.RegisterBuilder(events => Toggle.LoadFrom(() => "", events));
			storage.RegisterProjection(new AllToggles());

			_handler = new UpdateToggleTagsHandler(storage);
			var toggle = Toggle.CreateNew(() => "", "name", "desc");
			toggle.AddTag("existing");

			using (var session = storage.BeginSession())
				session.Save(toggle);

			_toggleID = toggle.ID;
		}

		private IEnumerable<Type> EventTypes() => _events[_toggleID].Select(e => e.GetType());
		private TEvent Event<TEvent>() => _events[_toggleID].OfType<TEvent>().Single();

		[Fact]
		public void When_the_toggle_doesnt_exist()
		{
			Should.Throw<KeyNotFoundException>(
				async () => await _handler.Handle(new UpdateToggleTagsRequest(ToggleID.CreateNew()))
			);
		}

		[Fact]
		public async Task When_there_are_no_tags_provided()
		{
			var response = await _handler.Handle(new UpdateToggleTagsRequest(_toggleID));

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded)
			});

			response.Tags.ShouldBe(new [] { "existing" });
		}

		[Fact]
		public async Task When_adding_a_tag()
		{
			var response = await _handler.Handle(new UpdateToggleTagsRequest(_toggleID)
			{
				Tags = new [] { "First" }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(TagAdded)
			});

			response.Tags.ShouldBe(new[] { "existing", "First" }, ignoreOrder: true);
		}

		[Fact]
		public async Task When_adding_multiple_tags()
		{
			var response = await _handler.Handle(new UpdateToggleTagsRequest(_toggleID)
			{
				Tags = new [] { "First", "Second", "Third" }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(TagAdded),
				typeof(TagAdded),
				typeof(TagAdded)
			});

			response.Tags.ShouldBe(new[] { "existing", "First", "Second", "Third" }, ignoreOrder: true);
		}
	}
}
