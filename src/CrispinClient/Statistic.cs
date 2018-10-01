using System;
using System.Collections.Generic;

namespace CrispinClient
{
	public class Statistic
	{
		public Guid ToggleID { get; set; }
		public string User { get; set; }
		public DateTime Timestamp { get; set; }
		public bool Active { get; set; }

		public Dictionary<int, bool> ConditionStates { get; set; }

		public Statistic()
		{
			ConditionStates = new Dictionary<int, bool>();
		}
	}
}
