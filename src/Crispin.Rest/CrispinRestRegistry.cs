using System.Collections.Generic;
using System.Linq;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Statistics;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
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
			});

			var store = BuildStorage();

			For<IStorage>().Use(store);
			For<IStatisticsWriter>().Use<CompositeStatisticsWriter>();
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
