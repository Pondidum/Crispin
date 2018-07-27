using System;
using System.Collections.Generic;
using System.Linq;
using FileSystem;

namespace Crispin.Infrastructure.Storage
{
	public class FileSystemStorage : IStorage
	{
		private readonly IFileSystem _fileSystem;
		private readonly string _root;
		private readonly Dictionary<Type, Projector> _projections;
		private readonly Dictionary<Type, Func<IEnumerable<IEvent>, AggregateRoot>> _builders;

		public FileSystemStorage(IFileSystem fileSystem, string root)
		{
			_fileSystem = fileSystem;
			_root = root;

			_builders = new Dictionary<Type, Func<IEnumerable<IEvent>, AggregateRoot>>();
			_projections = new Dictionary<Type, Projector>();
		}

		public void RegisterAggregate<TAggregate>() where TAggregate : AggregateRoot, new()
		{
			RegisterAggregate(() => new TAggregate());
		}

		public void RegisterAggregate<TAggregate>(Func<TAggregate> createBlank) where TAggregate : AggregateRoot
		{
			_builders[typeof(TAggregate)] = events =>
			{
				var instance = createBlank();
				var applicator = new Aggregator(typeof(TAggregate));
				applicator.Apply(instance, events);
				return instance;
			};
		}

		public void RegisterProjection<TProjection>() where TProjection : new()
		{
			RegisterProjection(() => new TProjection());
		}

		public void RegisterProjection<TProjection>(Func<TProjection> createBlank)
		{
			_projections.Add(typeof(TProjection), new Projector(typeof(TProjection), () => createBlank()));
		}

		public IStorageSession CreateSession() => new FileSystemSession(
			_fileSystem,
			_builders,
			_projections.Values,
			_root);
	}

	public class Projector
	{
		private readonly Func<object> _createBlank;
		private readonly Aggregator _aggregator;
		private Dictionary<ToggleID, object> _items;

		public Projector(Type projection, Func<object> createBlank)
		{
			_createBlank = createBlank;
			_aggregator = new Aggregator(projection);
			_items = new Dictionary<ToggleID, object>();
			For = projection;
		}

		public Type For { get; }

		public void Apply<TEvent>(TEvent @event) where TEvent : IEvent
		{
			object aggregate;

			if (_items.TryGetValue(@event.AggregateID, out aggregate) == false)
				_items[@event.AggregateID] = aggregate = _createBlank();

			var eventType = @event.GetType();
			var method = _aggregator.GetType().GetMethods()
				.Where(m => m.Name == nameof(Aggregator.Apply))
				.Where(m => m.IsGenericMethod)
				.Select(m => m.MakeGenericMethod(eventType))
				.Single();

			method.Invoke(_aggregator, new[] { aggregate, @event });
		}

		public Dictionary<ToggleID, object> ToMemento() => _items;
		public void FromMemento(Dictionary<ToggleID, object> items) => _items = items;
	}
}
