using System;
using Crispin.Infrastructure.Storage;
using Lamar;

namespace Crispin.Rest.Configuration
{
	public class CrispinRestRegistry : ServiceRegistry
	{
		public CrispinRestRegistry()
		{
			Scan(a =>
			{
				a.AssemblyContainingType<Toggle>();
				a.WithDefaultConventions();
			});

			For<Func<DateTime>>().Use(() => DateTime.UtcNow);
			For<IStorageSession>().Use(c => c.GetInstance<IStorage>().CreateSession()).Scoped();
		}
	}
}
