using System.Collections.Generic;
using System.Linq;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Statistics;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using Crispin.Rest.Infrastructure;
using MediatR;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;

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
				a.Convention<CompositeDecorator<CompositeStatisticsWriter, IStatisticsWriter>>();
			});

			var store = BuildStorage();

			For<IStorage>().Use(store);
		}

		private static InMemoryStorage BuildStorage()
		{
			var store = new InMemoryStorage();
			store.RegisterProjection(new AllToggles());
			store.RegisterBuilder(Toggle.LoadFrom);

			return store;
		}
	}
}
