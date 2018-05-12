using Crispin.Events;
using Crispin.Projections;
using Shouldly;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace Crispin.Tests.Projections
{
	public class AllTogglesTests
	{
		private readonly AllToggles _projection;
		private readonly EditorID _editor;

		public AllTogglesTests()
		{
			_projection = new AllToggles();
			_editor = EditorID.Parse("test");
		}

		[Fact]
		public void When_no_events_have_been_processed()
		{
			_projection.Toggles.ShouldBeEmpty();
		}

		[Fact]
		public void When_a_single_toggle_has_been_created()
		{
			var created = new ToggleCreated(_editor, ToggleID.CreateNew(), "toggle-1", "");
			_projection.Consume(created);

			var view = _projection.Toggles.Single();

			view.ShouldSatisfyAllConditions(
				() => view.ID.ShouldBe(created.ID),
				() => view.Name.ShouldBe(created.Name),
				() => view.Description.ShouldBe(created.Description),
				() => view.Tags.ShouldBeEmpty()
			);
		}

		[Fact]
		public void When_multiple_toggles_have_been_created()
		{
			var first = new ToggleCreated(_editor, ToggleID.CreateNew(), "toggle-1", "");
			var second = new ToggleCreated(_editor, ToggleID.CreateNew(), "toggle-1", "");
			var third = new ToggleCreated(_editor, ToggleID.CreateNew(), "toggle-1", "");

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
		public void When_a_toggle_has_a_tag_added()
		{
			var created = new ToggleCreated(_editor, ToggleID.CreateNew(), "toggle-1", "");

			_projection.Consume(created);
			_projection.Consume(new TagAdded(_editor, "one") { AggregateID = created.ID });

			_projection.Toggles.Single().Tags.ShouldBe(new[] { "one" });
		}

		[Fact]
		public void When_a_toggle_has_a_tag_removed()
		{
			var created = new ToggleCreated(_editor, ToggleID.CreateNew(), "toggle-1", "");

			_projection.Consume(created);
			_projection.Consume(new TagAdded(_editor, "one") { AggregateID = created.ID });
			_projection.Consume(new TagAdded(_editor, "two") { AggregateID = created.ID });
			_projection.Consume(new TagRemoved(_editor, "one") { AggregateID = created.ID });

			_projection.Toggles.Single().Tags.ShouldBe(new[] { "two" });
		}

		[Fact]
		public void When_deserializing()
		{
			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.None,
				TypeNameHandling = TypeNameHandling.Objects
			};

			var toggleID = ToggleID.CreateNew();
			_projection.Consume(new ToggleCreated(_editor, toggleID, "toggle-1", ""));

			var json = JsonConvert.SerializeObject(_projection.ToMemento(), settings);
			var memento = JsonConvert.DeserializeObject(json, settings) as AllTogglesMemento;

			memento.Single().Value.Name.ShouldBe("toggle-1");
		}
	}
}
