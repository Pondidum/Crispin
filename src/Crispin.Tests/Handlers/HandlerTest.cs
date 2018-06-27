using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;

namespace Crispin.Tests.Handlers
{
	public abstract class HandlerTest<THandler>
	{
		public InMemoryStorage Storage { get; }
		protected Toggle Toggle { get; }
		protected ToggleLocator Locator { get; }
		protected THandler Handler { get; }
		protected Dictionary<ToggleID, List<Event>> Events { get; }
		protected EditorID Editor { get; }

		public HandlerTest()
		{
			Events = new Dictionary<ToggleID, List<Event>>();

			Storage = new InMemoryStorage(Events);
			Storage.RegisterBuilder(Toggle.LoadFrom);
			Storage.RegisterProjection(new AllTogglesProjection());

			Handler = CreateHandler(Storage);

			var toggle = Toggle.CreateNew(EditorID.Parse("editor"), "name", "desc");
			InitialiseToggle(toggle);

			using (var session = Storage.BeginSession().Result)
				session.Save(toggle).Wait();

			Toggle = toggle;
			Locator = ToggleLocator.Create(toggle.ID);
			Editor = EditorID.Parse("Editor:" + Guid.NewGuid());
		}

		protected IEnumerable<Type> EventTypes() => Events[Toggle.ID].Select(e => e.GetType());
		protected TEvent Event<TEvent>() => Events[Toggle.ID].OfType<TEvent>().Single();
		protected void Event<TEvent>(Action<TEvent> callback) => callback(Event<TEvent>());

		protected abstract THandler CreateHandler(IStorage storage);

		protected virtual void InitialiseToggle(Toggle toggle)
		{
		}
	}
}
