using System;

namespace Crispin.Infrastructure.Storage
{
	public class AggregateNotFoundException : Exception
	{
		public AggregateNotFoundException(Type aggregateType, ToggleID aggregateID)
			: base($"Unable to find a {aggregateType.Name} with ID {aggregateID}")
		{
		}
	}
}
