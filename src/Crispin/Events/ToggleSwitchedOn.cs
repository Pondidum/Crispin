using System;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOn : ITimeStamped
	{
		public ToggleSwitchedOn()
		{
		}

		public DateTime TimeStamp { get; set; }
	}

	public class ToggleSwitchedOff : ITimeStamped
	{
		public ToggleSwitchedOff()
		{
		}

		public DateTime TimeStamp { get; set; }
	}
}
