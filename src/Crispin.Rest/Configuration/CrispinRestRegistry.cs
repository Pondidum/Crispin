using System;
using Crispin.Infrastructure.Storage;
using StructureMap;

namespace Crispin.Rest.Configuration
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

			For<Func<DateTime>>().Use<Func<DateTime>>(() => () => DateTime.UtcNow);
			For<IStorageSession>().Use(c => c.GetInstance<IStorage>().CreateSession());
		}
	}
}
