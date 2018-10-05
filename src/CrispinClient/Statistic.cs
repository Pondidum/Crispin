using System;
using System.Collections.Generic;
using CrispinClient.Conditions;

namespace CrispinClient
{
	public class Statistic
	{
		public Guid ToggleID { get; set; }
		public string User { get; set; }
		public DateTime Timestamp { get; set; }
		public bool Active { get; set; }
		public IEnumerable<KeyValuePair<int, bool>> ConditionStates => _states;

		private readonly Dictionary<int, bool> _states;

		public Statistic()
		{
			_states = new Dictionary<int, bool>();
		}

		public void Add(Condition condition, bool state)
		{
			_states[condition.ID] = state;
		}
	}
}
