using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Projections;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class InMemorySessionTests : StorageSessionTests
	{
		private readonly Dictionary<ToggleID, List<Event>> _eventStore;
		private readonly ToggleID _aggregateID;
		private readonly IGroupMembership _membership;
		private readonly EditorID _editor;

		public InMemorySessionTests()
		{
			_aggregateID = ToggleID.CreateNew();
			_eventStore = new Dictionary<ToggleID, List<Event>>();
			
			Session = new InMemorySession(Builders, Projections, _eventStore);
			_membership = Substitute.For<IGroupMembership>();
			_editor = EditorID.Parse("test editor");
		}


		protected override Task<bool> AggregateExists(ToggleID toggleID)
		{
			return Task.FromResult(_eventStore.ContainsKey(toggleID));
		}

		protected override Task WriteEvents(ToggleID toggleID, params object[] events)
		{
			_eventStore[toggleID] = events.Cast<Event>().ToList();
			return Task.CompletedTask;
		}

		protected override Task<IEnumerable<Type>> ReadEvents(ToggleID id)
		{
			return Task.FromResult(_eventStore[id].Select(e => e.GetType()));
		}

		protected override Task<TProjection> ReadProjection<TProjection>(TProjection projection)
		{
			return Task.FromResult(Projections.OfType<TProjection>().SingleOrDefault());
		}

		[Fact]
		public void When_there_is_a_projection_with_multiple_aggregates()
		{
			var projection = new AllToggles();
			Projections.Add(projection);

			var first = Toggle.CreateNew(_editor, "First", "yes");
			var second = Toggle.CreateNew(_editor, "Second", "yes");

			Session.Save(first);
			Session.Save(second);
			Session.Commit();

			projection.Toggles.Select(v => v.ID).ShouldBe(new[]
			{
				first.ID,
				second.ID
			}, ignoreOrder: true);
		}

		[Fact]
		public void When_retrieving_a_projection_which_exists_in_the_session()
		{
			var projection = new AllToggles();
			Projections.Add(projection);

			Session.LoadProjection<AllToggles>()
				.ShouldBe(projection);
		}

		[Fact]
		public void When_retrieving_a_projection_which_doesnt_exist_in_the_session()
		{
			Should.Throw<ProjectionNotRegisteredException>(
				() => Session.LoadProjection<AllToggles>()
			);
		}
	}
}
