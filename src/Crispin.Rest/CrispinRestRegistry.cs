using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using MediatR;
using StructureMap;

namespace Crispin.Rest
{
	public class CrispinRestRegistry : Registry
	{
		public CrispinRestRegistry()
		{
			Scan(a =>
			{
				a.AssemblyContainingType<Toggle>();
				a.WithDefaultConventions();
			});

			var store = BuildStorage();

			For<IStorage>().Use(store);
			For(typeof(IPipelineBehavior<,>)).Use(typeof(TimingBehavior<,>));
		}

		private static InMemoryStorage BuildStorage()
		{
			var store = new InMemoryStorage();
			store.RegisterProjection(new AllToggles());
			store.RegisterBuilder(events => Toggle.LoadFrom(() => "", events));

			return store;
		}
	}
}
