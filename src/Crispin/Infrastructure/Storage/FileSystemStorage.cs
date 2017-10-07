using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
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
		private readonly string _root;
		private readonly Dictionary<ToggleID, List<Event>> _pending;

		public FileSystemSession(IFileSystem fileSystem, Dictionary<Type, Func<IEnumerable<Event>, AggregateRoot>> builders, string root)
		{
			_fileSystem = fileSystem;
			_builders = builders;
			_root = root;

			_pending = new Dictionary<ToggleID, List<Event>>();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void Open()
		{
			throw new NotImplementedException();
		}

		public TProjection LoadProjection<TProjection>() where TProjection : Projection
		{
			throw new NotImplementedException();
		}

		public TAggregate LoadAggregate<TAggregate>(ToggleID aggregateID) where TAggregate : AggregateRoot
		{
			try
			{
				return LoadAggregateInternal<TAggregate>(aggregateID).Result;
			}
			catch (AggregateException e)
			{
				if (e.InnerException != null)
					throw e.InnerException;
				else
					throw;
			}
		}

		private async Task<TAggregate> LoadAggregateInternal<TAggregate>(ToggleID aggregateID) where TAggregate : AggregateRoot
		{
			Func<IEnumerable<Event>, AggregateRoot> builder;

			if (_builders.TryGetValue(typeof(TAggregate), out builder) == false)
				throw new NotSupportedException($"No builder for type {typeof(TAggregate).Name} found.");

			var aggregatePath = Path.Combine(_root, aggregateID.ToString());

			if (await _fileSystem.FileExists(aggregatePath) == false)
				throw new KeyNotFoundException($"Unable to find an aggregate with ID {aggregateID}");

			var events = (await _fileSystem.ReadFileLines(aggregatePath))
				.Select(e => JsonConvert.DeserializeObject(e, JsonSerializerSettings))
				.Cast<Event>()
				.ToArray();

			if (events.Any() == false)
				throw new KeyNotFoundException($"Unable to find an aggregate with ID {aggregateID}");

			var aggregate = builder(events.ToList());

			return (TAggregate)aggregate;
		}

		public void Save<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot, IEvented
		{
			var pending = aggregate.GetPendingEvents().Cast<Event>();

			if (_pending.ContainsKey(aggregate.ID) == false)
				_pending[aggregate.ID] = new List<Event>();

			_pending[aggregate.ID].AddRange(pending);
			aggregate.ClearPendingEvents();
		}

		public void Commit()
		{
			foreach (var pair in _pending)
			{
				var path = Path.Combine(_root, pair.Key.ToString());
				var events = pair.Value.Select(e => JsonConvert.SerializeObject(e, JsonSerializerSettings));

				_fileSystem.AppendFile(path, stream =>
				{
					using (var writer = new StreamWriter(stream))
						events.Each(e => writer.WriteLine(e));

					return Task.CompletedTask;
				}).Wait();
			}
		}
	}
}
