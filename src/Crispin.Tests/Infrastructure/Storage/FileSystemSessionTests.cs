using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using FileSystem;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class FileSystemSessionTests : StorageSessionTests
	{
		private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		{
			Formatting = Formatting.None,
			TypeNameHandling = TypeNameHandling.Objects
		};

		private const string Root = "./store";
		private readonly InMemoryFileSystem _fs;

		public FileSystemSessionTests()
		{
			_fs = new InMemoryFileSystem();
		}

		protected override async Task<IStorageSession> CreateSession()
		{
			await _fs.CreateDirectory(Root);
			return new FileSystemSession(_fs, Builders, Projections, Root);
		}

		protected override async Task<bool> AggregateExists(ToggleID toggleID)
		{
			return await _fs.FileExists(Path.Combine(Root, toggleID.ToString()));
		}

		protected override async Task WriteEvents(ToggleID toggleID, params IEvent[] events)
		{
			await _fs.WriteFile(Path.Combine(Root, toggleID.ToString()), stream =>
			{
				using (var writer = new StreamWriter(stream))
					events
						.Select(e => JsonConvert.SerializeObject(e, JsonSettings))
						.Each(line => writer.WriteLine(line));

				return Task.CompletedTask;
			});
		}

		protected override async Task<IEnumerable<Type>> ReadEvents(ToggleID toggleID)
		{
			var path = Path.Combine(Root, toggleID.ToString());

			if (await _fs.FileExists(path) == false)
				return Enumerable.Empty<Type>();

			var lines = await _fs.ReadFileLines(path);

			return lines
				.Select(line => JsonConvert.DeserializeObject(line, JsonSettings))
				.Select(e => e.GetType());
		}

		protected override async Task<IEnumerable<TProjection>> ReadProjection<TProjection>()
		{
			var path = Path.Combine(Root, typeof(TProjection).Name + ".json");

			using (var stream = await _fs.ReadFile(path))
			using (var reader = new StreamReader(stream))
			{
				var json = await reader.ReadToEndAsync();
				var memento = JsonConvert.DeserializeObject<Dictionary<object, object>>(json, JsonSettings);

				return memento.Values.Cast<TProjection>();
			}
		}

		[Fact]
		public async Task When_the_session_is_opened()
		{
			await Session.Open();

			var exists = await _fs.DirectoryExists(Root);
			exists.ShouldBe(true);
		}
	}
}
