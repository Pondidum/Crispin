using System;

namespace CrispinClient
{
	public class ToggleNotFoundException : Exception
	{
		public ToggleNotFoundException(Guid toggleID)
			: base($"Unable to find a toggle with the ID '{toggleID}'")
		{
		}
	}
}
