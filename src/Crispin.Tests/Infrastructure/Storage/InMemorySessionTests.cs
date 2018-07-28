using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Views;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class InMemorySessionTests : StorageSessionTests
	{
		private readonly Dictionary<ToggleID, List<IAct>> _eventStore;

		public InMemorySessionTests()
		{
			_eventStore = new Dictionary<ToggleID, List<IAct>>();
		}

		protected override Task<IStorageSession> CreateSession()
		{
			return Task.FromResult((IStorageSession)new InMemorySession(Builders, Projections, _eventStore));
		}

		protected override Task<bool> AggregateExists(ToggleID toggleID)
		{
			return Task.FromResult(_eventStore.ContainsKey(toggleID));
		}

		protected override Task WriteEvents(ToggleID toggleID, params IAct[] events)
		{
			_eventStore[toggleID] = events.ToList();
			return Task.CompletedTask;
		}

		protected override Task<IEnumerable<Type>> ReadEvents(ToggleID toggleID)
		{
			if (_eventStore.ContainsKey(toggleID) == false)
				return Task.FromResult(Enumerable.Empty<Type>());

			return Task.FromResult(_eventStore[toggleID].Select(e => e.GetType()));
		}

		protected override Task<IEnumerable<TProjection>> ReadProjection<TProjection>()
		{
			return Task.FromResult(Projections.SingleOrDefault(p => p.For == typeof(TProjection))?.ToMemento().Values.Cast<TProjection>());
		}

		[Fact]
		public async Task When_there_is_a_projection_with_multiple_aggregates()
		{
			AddDefaultProjection();

			var first = Toggle.CreateNew(Editor, "First", "yes");
			var second = Toggle.CreateNew(Editor, "Second", "yes");

			await Session.Save(first);
			await Session.Save(second);
			await Session.Commit();

			var projection = await Session.QueryProjection<ToggleView>();
			projection.Select(v => v.ID).ShouldBe(new[]
			{
				first.ID,
				second.ID
			}, ignoreOrder: true);
		}

		[Fact]
		public async Task When_retrieving_a_projection_which_exists_in_the_session()
		{
			AddDefaultProjection();

			var loadProjection = await Session.QueryProjection<ToggleView>();

			loadProjection.ShouldNotBeNull();
		}

		[Fact]
		public void When_retrieving_a_projection_which_doesnt_exist_in_the_session()
		{
			Should.Throw<ProjectionNotRegisteredException>(
				() => Session.QueryProjection<ToggleView>()
			);
		}
	}
}
