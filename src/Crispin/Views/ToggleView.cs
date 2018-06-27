using System;
using System.Collections.Generic;
using Crispin.Conditions;

namespace Crispin.Views
{
	public class ToggleView
	{
		public ToggleID ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HashSet<string> Tags { get; set; }
		public List<Condition> Conditions { get; set; }
		public ConditionModes ConditionMode { get; set; }

		private readonly ConditionBuilder _builder;

		public ToggleView()
		{
			Tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			Conditions = new List<Condition>();

			_builder = new ConditionBuilder(Conditions);
		}

		public void AddCondition(Condition condition, ConditionID parent) => _builder.Add(condition, parent);
		public void RemoveCondition(ConditionID conditionID) => _builder.Remove(conditionID);
	}
}
