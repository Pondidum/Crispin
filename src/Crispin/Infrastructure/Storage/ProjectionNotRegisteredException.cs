using System;

namespace Crispin.Infrastructure.Storage
{
	public class ProjectionNotRegisteredException : Exception
	{
		public ProjectionNotRegisteredException(string projectionName)
			: base($"There was no projection registered called {projectionName}.\n\nDid you forget to call IStorage.RegisterProjection({projectionName})?")
		{
		}
	}
}
