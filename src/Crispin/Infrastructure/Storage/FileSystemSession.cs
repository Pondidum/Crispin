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
		private readonly Dictionary<Type, Func<IEnumerable<IEvent>, object>> _builders;
		private readonly IEnumerable<Projector> _projections;
		private readonly string _root;
		private readonly PendingEventsStore _pending;

		public FileSystemSession(
			IFileSystem fileSystem,
			Dictionary<Type, Func<IEnumerable<IEvent>, object>> builders,
			IEnumerable<Projector> projections,
			string root)
		{
			_fileSystem = fileSystem;
			_builders = builders;
			_projections = projections;
			_root = root;

			_pending = new PendingEventsStore();
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

		public async Task<IEnumerable<TProjection>> QueryProjection<TProjection>()
		{
			var projection = _projections.FirstOrDefault(p => p.For == typeof(TProjection));

			if (projection == null)
				throw new ProjectionNotRegisteredException(typeof(TProjection).Name);

			var projectionPath = Path.Combine(_root, projection.GetType().Name + ".json");

			if (await _fileSystem.FileExists(projectionPath) == false)
				return projection.ToMemento().Values.Cast<TProjection>();

			using (var stream = await _fileSystem.ReadFile(projectionPath))
			using (var reader = new StreamReader(stream))
			{
				var json = await reader.ReadToEndAsync();
				var memento = JsonConvert.DeserializeObject<Dictionary<object, object>>(json, JsonSerializerSettings);

				projection.FromMemento(memento);
			}

			return projection.ToMemento().Values.Cast<TProjection>();
		}

		public async Task<TAggregate> LoadAggregate<TAggregate>(object aggregateID)
		{
			Func<IEnumerable<IEvent>, object> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new BuilderNotFoundException(_builders.Keys, typeof(TAggregate));

			var aggregatePath = Path.Combine(_root, aggregateID.ToString());

			var fsEvents = await _fileSystem.FileExists(aggregatePath)
				? (await _fileSystem.ReadFileLines(aggregatePath))
					.Select(e => JsonConvert.DeserializeObject(e, JsonSerializerSettings))
					.Cast<IEvent>()
				: Enumerable.Empty<IEvent>();

			var sessionEvents = _pending.EventsFor<TAggregate>(aggregateID);

			var events = fsEvents
				.Concat(sessionEvents)
				.ToArray();

			if (events.Any() == false)
				throw new AggregateNotFoundException(typeof(TAggregate), aggregateID);

			var aggregate = builder(events);

			return (TAggregate)aggregate;
		}

		public Task Save<TAggregate>(TAggregate aggregate) where TAggregate : IEvented
		{
			_pending.AddEvents<TAggregate>(aggregate.GetPendingEvents());

			aggregate.ClearPendingEvents();

			return Task.CompletedTask;
		}

		public async Task Commit()
		{
			await _pending.ForEach(async (aggregateType, aggregateID, events) =>
			{
				var aggregatePath = Path.Combine(_root, aggregateID.ToString());
				var lines = events
					.Select(e => JsonConvert.SerializeObject(e, JsonSerializerSettings))
					.ToArray();

				await _fileSystem.AppendFileLines(aggregatePath, lines);
			});


			var eventsForProjection = _pending.AllEvents.ToArray();

			foreach (var projection in _projections)
			foreach (var @event in eventsForProjection)
			{
				projection.Apply(@event);

				var projectionPath = Path.Combine(_root, projection.For.Name + ".json");
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
