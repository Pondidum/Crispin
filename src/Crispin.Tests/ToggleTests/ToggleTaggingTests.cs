using System;
using System.Collections.Generic;
using Crispin.Events;
using System.Linq;
using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleTaggingTests
	{
		private Toggle _toggle;

		public void CreateToggle(params object[] events)
		{
			var create = new ToggleCreated(Guid.NewGuid(), "Test Toggle", "");

			_toggle = Toggle.LoadFrom(
				() => string.Empty,
				new[] { create }.Concat(events));
		}

		private IEnumerable<object> Events => ((IEvented)_toggle).GetPendingEvents();
		private TEvent SingleEvent<TEvent>() => (TEvent)Events.Single();

		[Fact]
		public void When_adding_a_new_tag_to_a_toggle()
		{
			CreateToggle();
			_toggle.AddTag("first-tag");

			SingleEvent<TagAdded>().Name.ShouldBe("first-tag");
			_toggle.Tags.ShouldBe(new [] { "first-tag" });
		}

		[Fact]
		public void When_adding_an_existing_tag_to_a_toggle()
		{
			CreateToggle(new TagAdded("first-tag"));
			_toggle.AddTag("first-tag");

			Events.ShouldBeEmpty();
			_toggle.Tags.ShouldBe(new [] { "first-tag" });
		}

		[Fact]
		public void When_removing_a_non_existing_tag_to_a_toggle()
		{
			CreateToggle();
			_toggle.RemoveTag("something");

			Events.ShouldBeEmpty();
			_toggle.Tags.ShouldBeEmpty();
		}

		[Fact]
		public void When_removing_an_existing_tag_to_a_toggle()
		{
			CreateToggle(new TagAdded("something"));
			_toggle.RemoveTag("something");

			SingleEvent<TagRemoved>().Name.ShouldBe("something");
			_toggle.Tags.ShouldBeEmpty();
		}

		[Fact]
		public void When_adding_a_toggle_which_differs_by_case()
		{
			CreateToggle(new TagAdded("testing"));
			_toggle.AddTag("TESTING");

			Events.ShouldBeEmpty();
			_toggle.Tags.ShouldBe(new[] { "testing" });
		}

		[Fact]
		public void When_removing_a_toggle_which_differs_by_case()
		{
			CreateToggle(new TagAdded("testing"));
			_toggle.RemoveTag("TESTING");

			SingleEvent<TagRemoved>().Name.ShouldBe("TESTING");
			_toggle.Tags.ShouldBeEmpty();
		}
	}
}
