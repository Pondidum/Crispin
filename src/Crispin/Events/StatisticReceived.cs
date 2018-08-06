using System;
using System.Collections.Generic;
using Crispin.Conditions;

namespace Crispin.Events
{
	public class StatisticReceived
	{
		public UserID User { get; }
		public DateTime Timestamp { get; }
		public bool Active { get; }
		public Dictionary<ConditionID, bool> ConditionStates { get; }

		public StatisticReceived(UserID user, DateTime timestamp, bool active, Dictionary<ConditionID, bool> conditionStates)
		{
			User = user;
			Timestamp = timestamp;
			Active = active;
			ConditionStates = conditionStates;
		}

		public override string ToString() => $"Toggle was {(Active ? "Active" : "Inactive")} for User '{User}'";
	}
}
