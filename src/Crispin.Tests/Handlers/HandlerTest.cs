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
		protected ToggleID ToggleID { get; }
		protected THandler Handler { get; }
		protected Dictionary<ToggleID, List<Event>> Events { get; }

		public HandlerTest()
		{
			Events = new Dictionary<ToggleID, List<Event>>();

			var storage = new InMemoryStorage(Events);
			storage.RegisterBuilder(events => Toggle.LoadFrom(() => "", events));
			storage.RegisterProjection(new AllToggles());

			Handler = CreateHandler(storage);

			var toggle = Toggle.CreateNew(() => "", "name", "desc");
			InitialiseToggle(toggle);

			using (var session = storage.BeginSession())
				session.Save(toggle);

			ToggleID = toggle.ID;
		}


		protected IEnumerable<Type> EventTypes() => Events[ToggleID].Select(e => e.GetType());
		protected TEvent Event<TEvent>() => Events[ToggleID].OfType<TEvent>().Single();

		protected abstract THandler CreateHandler(IStorage storage);

		protected virtual void InitialiseToggle(Toggle toggle)
		{
		}
	}
}
