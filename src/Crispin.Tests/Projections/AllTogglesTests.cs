using System;
using System.Collections.Generic;
using Crispin.Events;
using Crispin.Projections;
using Shouldly;
using System.Linq;
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
			var created = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");
			_projection.Consume(created);

			var view = _projection.Toggles.Single();

			view.ShouldSatisfyAllConditions(
				() => view.ID.ShouldBe(created.ID),
				() => view.Name.ShouldBe(created.Name),
				() => view.Description.ShouldBe(created.Description),
				() => view.Tags.ShouldBeEmpty(),
				() => view.State.Anonymous.ShouldBeFalse(),
				() => view.State.Users.ShouldBeEmpty(),
				() => view.State.Groups.ShouldBeEmpty()
			);
		}

		[Fact]
		public void When_multiple_toggles_have_been_created()
		{
			var first = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");
			var second = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");
			var third = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");

			_projection.Consume(first);
			_projection.Consume(second);
			_projection.Consume(third);

			_projection.Toggles.Select(v => v.ID).ShouldBe(new[]
			{
				first.ID,
				second.ID,
				third.ID
			}, ignoreOrder: true);
		}

		[Fact]
		public void When_a_toggle_is_switched_on()
		{
			var created = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");

			_projection.Consume(created);
			_projection.Consume(new ToggleSwitchedOn(UserID.Empty) { AggregateID = created.ID });

			_projection.Toggles.Single().State.Anonymous.ShouldBe(true);
		}

		[Fact]
		public void When_a_toggle_is_switched_off()
		{
			var created = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");

			_projection.Consume(created);
			_projection.Consume(new ToggleSwitchedOn(UserID.Empty) { AggregateID = created.ID });
			_projection.Consume(new ToggleSwitchedOff(UserID.Empty) { AggregateID = created.ID });

			_projection.Toggles.Single().State.Anonymous.ShouldBe(false);
		}

		[Fact]
		public void When_a_toggle_has_a_tag_added()
		{
			var created = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");

			_projection.Consume(created);
			_projection.Consume(new TagAdded("one") { AggregateID = created.ID });

			_projection.Toggles.Single().Tags.ShouldBe(new[] { "one" });
		}

		[Fact]
		public void When_a_toggle_has_a_tag_removed()
		{
			var created = new ToggleCreated(ToggleID.CreateNew(), "toggle-1", "");

			_projection.Consume(created);
			_projection.Consume(new TagAdded("one") { AggregateID = created.ID });
			_projection.Consume(new TagAdded("two") { AggregateID = created.ID });
			_projection.Consume(new TagRemoved("one") { AggregateID = created.ID });

			_projection.Toggles.Single().Tags.ShouldBe(new[] { "two" });
		}
	}
}
