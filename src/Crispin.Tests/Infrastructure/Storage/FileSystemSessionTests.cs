using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using FileSystem;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class FileSystemSessionTests
	{
		private const string Root = "./store";

		private readonly InMemoryFileSystem _fs;
		private readonly FileSystemSession _session;
		private readonly Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> _builders;
		private readonly ToggleID _aggregateID;
		private readonly EditorID _editor;
		private readonly IGroupMembership _membership;
		private readonly List<IProjection> _projections;

		public FileSystemSessionTests()
		{
			_builders = new Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>>
			{
				{ typeof(Toggle), Toggle.LoadFrom }
			};
			_projections = new List<IProjection>();

			_fs = new InMemoryFileSystem();
			_fs.CreateDirectory(Root).Wait();

			_session = new FileSystemSession(_fs, _builders, _projections, Root);
			_membership = Substitute.For<IGroupMembership>();
			_editor = EditorID.Parse("wat");
			_aggregateID = ToggleID.CreateNew();
		}


		[Fact]
		public void When_there_is_no_builder_for_an_aggregate()
		{
			_builders.Clear();

			Should.Throw<NotSupportedException>(() => _session.LoadAggregate<Toggle>(_aggregateID));
		}

		[Fact]
		public void When_there_is_no_aggregate_stored()
		{
			Should.Throw<KeyNotFoundException>(() => _session.LoadAggregate<Toggle>(_aggregateID));
		}

		[Fact]
		public async Task When_there_are_no_events_for_an_aggregate_stored()
		{
			await _fs.WriteFile(
				Path.Combine(Root, _aggregateID.ToString()),
				stream => Task.CompletedTask);

			Should.Throw<KeyNotFoundException>(() => _session.LoadAggregate<Toggle>(_aggregateID));
		}

		[Fact]
		public async Task When_an_aggregate_is_loaded()
		{
			await WriteEvents(
				new ToggleCreated(_editor, _aggregateID, "First", "hi"),
				new TagAdded(_editor, "one"),
				new ToggleSwitchedOnForUser(_editor, UserID.Parse("user-1"))
			);

			var toggle = _session.LoadAggregate<Toggle>(_aggregateID);

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(_aggregateID),
				() => toggle.IsActive(_membership, UserID.Parse("user-1")).ShouldBeTrue(),
				() => toggle.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public async Task When_saving_an_aggregate_and_commit_is_not_called()
		{
			var toggle = Toggle.CreateNew(_editor, "First", "hi");
			toggle.AddTag(_editor, "one");
			toggle.ChangeDefaultState(_editor, newState: States.On);

			_session.Save(toggle);

			var exists = await _fs.FileExists(Path.Combine(Root, _aggregateID.ToString()));

			exists.ShouldBeFalse();
		}

		[Fact]
		public async Task When_saving_an_aggregate_and_commit_is_called()
		{
			var toggle = Toggle.CreateNew(_editor, "First", "hi");
			toggle.AddTag(_editor, "one");
			toggle.ChangeDefaultState(_editor, newState: States.On);

			_session.Save(toggle);
			_session.Commit();

			var events = await ReadEvents(toggle.ID);

			events.ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(ToggleSwitchedOnForAnonymous)
			});
		}

		[Fact]
		public void When_loading_an_aggregate_saved_in_the_current_session()
		{
			var toggle = Toggle.CreateNew(_editor, "First", "hi");
			toggle.AddTag(_editor, "one");
			toggle.ChangeState(_editor, UserID.Parse("user-1"), States.On);

			_session.Save(toggle);

			var loaded = _session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.IsActive(_membership, UserID.Parse("user-1")).ShouldBe(true),
				() => loaded.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public void When_loading_an_aggregate_existing_in_store_and_saved_in_the_current_session()
		{
			var toggle = Toggle.CreateNew(_editor, "First", "hi");
			toggle.AddTag(_editor, "one");

			_session.Save(toggle);
			_session.Commit();

			toggle.ChangeState(_editor, UserID.Parse("user-1"), States.On);
			_session.Save(toggle);

			var loaded = _session.LoadAggregate<Toggle>(toggle.ID);

			loaded.ShouldSatisfyAllConditions(
				() => loaded.ID.ShouldBe(toggle.ID),
				() => loaded.IsActive(_membership, UserID.Parse("user-1")).ShouldBe(true),
				() => loaded.Tags.ShouldContain("one")
			);
		}

		[Fact]
		public async Task When_there_are_pending_events_and_dispose_is_called()
		{
			var toggle = Toggle.CreateNew(_editor, "First", "hi");
			toggle.AddTag(_editor, "one");

			_session.Save(toggle);
			_session.Dispose();

			var events = await ReadEvents(toggle.ID);

			events.ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded)
			});
		}

		[Fact]
		public async Task When_commit_is_called_twice()
		{
			var toggle = Toggle.CreateNew(_editor, "First", "hi");
			toggle.AddTag(_editor, "one");

			_session.Save(toggle);
			_session.Commit();

			var before = await ReadEvents(toggle.ID);
			before.Count().ShouldBe(2);

			_session.Commit();

			var after = await ReadEvents(toggle.ID);
			after.Count().ShouldBe(2);
		}

		[Fact]
		public async Task When_there_is_a_projection()
		{
			var projection = new AllToggles();
			_projections.Add(projection);

			var toggle = Toggle.CreateNew(_editor, "Projected", "yes");

			_session.Save(toggle);
			_session.Commit();

			var view = projection.Toggles.Single();

			view.ShouldSatisfyAllConditions(
				() => view.ID.ShouldBe(toggle.ID),
				() => view.Name.ShouldBe(toggle.Name),
				() => view.Description.ShouldBe(toggle.Description),
				() => view.Tags.ShouldBeEmpty(),
				() => view.State.Anonymous.ShouldBe(States.Off),
				() => view.State.Users.ShouldBeEmpty(),
				() => view.State.Groups.ShouldBeEmpty()
			);

			var fromDisk = await ReadProjection<AllToggles>();

			fromDisk.Toggles.Select(t => t.ID).ShouldBe(projection.Toggles.Select(t => t.ID));
		}


		private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		{
			Formatting = Formatting.None,
			TypeNameHandling = TypeNameHandling.Objects
		};

		private async Task WriteEvents(params object[] events)
		{
			await _fs.WriteFile(Path.Combine(Root, _aggregateID.ToString()), stream =>
			{
				using (var writer = new StreamWriter(stream))
					events
						.Select(e => JsonConvert.SerializeObject(e, JsonSettings))
						.Each(line => writer.WriteLine(line));

				return Task.CompletedTask;
			});
		}

		private async Task<IEnumerable<Type>> ReadEvents(ToggleID id)
		{
			var lines = (await _fs.ReadFileLines(Path.Combine(Root, id.ToString())));

			return lines
				.Select(line => JsonConvert.DeserializeObject(line, JsonSettings))
				.Select(e => e.GetType());
		}

		private async Task<TProjection> ReadProjection<TProjection>()
		{
			var path = Path.Combine(Root, typeof(TProjection).Name + ".json");

			using (var stream = await _fs.ReadFile(path))
			using (var reader = new StreamReader(stream))
			{
				return (TProjection)JsonConvert.DeserializeObject(
					await reader.ReadToEndAsync(),
					JsonSettings);
			}
		}
	}
}
