using System;
using System.Collections.Generic;
using Crispin.Events;
using Crispin.Projections;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Projections
{
	public class AllTogglesTests
	{
		private readonly AllToggles _projection;

		public AllTogglesTests()
		{
			_projection = new AllToggles();
		}

		[Fact]
		public void When_no_events_have_been_processed()
		{
			_projection.Toggles.ShouldBeEmpty();
		}

		[Fact]
		public void When_a_single_toggle_has_been_created()
		{
			var created = new ToggleCreated(Guid.NewGuid(), "toggle-1", "");
			_projection.Consume(created);

			_projection.Toggles.ShouldBe(new Dictionary<Guid, string>
			{
				{ created.ID, created.Name }
			});
		}

		[Fact]
		public void When_multiple_toggles_have_been_created()
		{
			var first = new ToggleCreated(Guid.NewGuid(), "toggle-1", "");
			var second = new ToggleCreated(Guid.NewGuid(), "toggle-1", "");
			var third = new ToggleCreated(Guid.NewGuid(), "toggle-1", "");

			_projection.Consume(first);
			_projection.Consume(second);
			_projection.Consume(third);

			_projection.Toggles.ShouldBe(new Dictionary<Guid, string>
			{
				{ first.ID, first.Name },
				{ second.ID, second.Name },
				{ third.ID, third.Name }
			});
		}

		[Fact]
		public void When_a_toggle_is_switched_on()
		{
			var created = new ToggleCreated(Guid.NewGuid(), "toggle-1", "");

			_projection.Consume(created);
			_projection.Consume(new ToggleSwitchedOn());

			_projection.Toggles.ShouldBe(new Dictionary<Guid, string>
			{
				{ created.ID, created.Name }
			});
		}
	}
}
