using System;
using System.Collections.Generic;
using Crispin.Conditions;

namespace Crispin
{
	public class Statistic
	{
		public ToggleID ToggleID { get; set; }
		public UserID User { get; set; }
		public DateTime Timestamp { get; set; }
		public bool Active { get; set; }

		public Dictionary<ConditionID, bool> ConditionStates { get; set; }
	}
}
