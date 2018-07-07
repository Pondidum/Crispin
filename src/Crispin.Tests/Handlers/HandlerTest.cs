using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public abstract class HandlerTest<THandler> : IAsyncLifetime
	{
		protected InMemoryStorage Storage { get; }
		protected IStorageSession Session { get; private set; }
		
		protected Toggle Toggle { get; }
		protected ToggleLocator Locator { get; }
		protected THandler Handler { get; private set; }
		protected Dictionary<ToggleID, List<Event>> Events { get; }
		protected EditorID Editor { get; }

		protected HandlerTest()
		{
			Events = new Dictionary<ToggleID, List<Event>>();

			Storage = new InMemoryStorage(Events);
			Storage.RegisterBuilder(Toggle.LoadFrom);
			Storage.RegisterProjection(new AllTogglesProjection());

			Toggle = Toggle.CreateNew(EditorID.Parse("editor"), "name", "desc");
			Locator = ToggleLocator.Create(Toggle.ID);
			Editor = EditorID.Parse("Editor:" + Guid.NewGuid());
		}

		public async Task InitializeAsync()
		{
			InitialiseToggle(Toggle);

			using (var session = Storage.CreateSession())
				await session.Save(Toggle);

			Session = Storage.CreateSession();
			Handler = CreateHandler(Session);
		}

		protected IEnumerable<Type> EventTypes() => Events[Toggle.ID].Select(e => e.GetType());
		protected TEvent Event<TEvent>() => Events[Toggle.ID].OfType<TEvent>().Single();
		protected void Event<TEvent>(Action<TEvent> callback) => callback(Event<TEvent>());

		protected abstract THandler CreateHandler(IStorageSession session);

		protected virtual void InitialiseToggle(Toggle toggle)
		{
		}

		public async Task DisposeAsync()
		{
			await Session.Commit();
			Session.Dispose();
		}
	}
}
