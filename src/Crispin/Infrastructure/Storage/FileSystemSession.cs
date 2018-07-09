using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileSystem;
using Newtonsoft.Json;

namespace Crispin.Infrastructure.Storage
{
	public class FileSystemSession : IStorageSession
	{
		private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
		{
			Formatting = Formatting.None,
			TypeNameHandling = TypeNameHandling.Objects
		};

		private readonly IFileSystem _fileSystem;
		private readonly Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> _builders;
		private readonly List<IProjection> _projections;
		private readonly string _root;
		private readonly Dictionary<ToggleID, List<Event>> _pending;

		public FileSystemSession(
			IFileSystem fileSystem,
			Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> builders,
			List<IProjection> projections,
			string root)
		{
			_fileSystem = fileSystem;
			_builders = builders;
			_projections = projections;
			_root = root;

			_pending = new Dictionary<ToggleID, List<Event>>();
		}

		public void Dispose()
		{
			if (_pending.Any())
				Commit().Wait();
		}

		public async Task Open()
		{
			await _fileSystem.CreateDirectory(_root);
		}

		public async Task<TProjection> LoadProjection<TProjection>() where TProjection : IProjection
		{
			var projection = _projections.OfType<TProjection>().FirstOrDefault();

			if (projection == null)
				throw new ProjectionNotRegisteredException(typeof(TProjection).Name);

			var projectionPath = Path.Combine(_root, projection.GetType().Name + ".json");

			if (await _fileSystem.FileExists(projectionPath) == false)
				return projection;

			using (var stream = await _fileSystem.ReadFile(projectionPath))
			using (var reader = new StreamReader(stream))
			{
				var json = await reader.ReadToEndAsync();
				var memento = JsonConvert.DeserializeObject(json, JsonSerializerSettings);

				projection.FromMemento(memento);
			}

			return projection;
		}

		public async Task<TAggregate> LoadAggregate<TAggregate>(ToggleID aggregateID) where TAggregate : AggregateRoot
		{
			Func<IEnumerable<Event>, AggregateRoot> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new BuilderNotFoundException(_builders.Keys, typeof(TAggregate));

			var aggregatePath = Path.Combine(_root, aggregateID.ToString());

			var fsEvents = await _fileSystem.FileExists(aggregatePath)
				? (await _fileSystem.ReadFileLines(aggregatePath))
				.Select(e => JsonConvert.DeserializeObject(e, JsonSerializerSettings))
				.Cast<Event>()
				: Enumerable.Empty<Event>();

			var sessionEvents = _pending.ContainsKey(aggregateID)
				? _pending[aggregateID]
				: Enumerable.Empty<Event>();

			var events = fsEvents
				.Concat(sessionEvents)
				.ToArray();

			if (events.Any() == false)
				throw new AggregateNotFoundException(typeof(TAggregate), aggregateID);

			var aggregate = builder(events);

			return (TAggregate)aggregate;
		}

		public Task Save<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot, IEvented
		{
			var pending = aggregate.GetPendingEvents().Cast<Event>();

			if (_pending.ContainsKey(aggregate.ID) == false)
				_pending[aggregate.ID] = new List<Event>();

			_pending[aggregate.ID].AddRange(pending);
			aggregate.ClearPendingEvents();

			return Task.CompletedTask;
		}

		public async Task Commit()
		{
			foreach (var pair in _pending)
			{
				var aggregatePath = Path.Combine(_root, pair.Key.ToString());
				var events = pair.Value.Select(e => JsonConvert.SerializeObject(e, JsonSerializerSettings));

				await _fileSystem.AppendFile(aggregatePath, async stream =>
				{
					using (var writer = new StreamWriter(stream))
						foreach (var @event in events)
							await writer.WriteLineAsync(@event);
				});
			}

			var eventsForProjection = _pending
				.SelectMany(p => p.Value)
				.ToArray();

			foreach (var projection in _projections)
			foreach (var @event in eventsForProjection)
			{
				projection.Consume(@event);

				var projectionPath = Path.Combine(_root, projection.GetType().Name + ".json");
				var projectionJson = JsonConvert.SerializeObject(projection.ToMemento(), JsonSerializerSettings);

				await _fileSystem.WriteFile(projectionPath, async stream =>
				{
					using (var writer = new StreamWriter(stream))
						await writer.WriteAsync(projectionJson);
				});
			}

			_pending.Clear();
		}

		public Task Abort()
		{
			_pending.Clear();
			return Task.CompletedTask;
		}
	}
}
