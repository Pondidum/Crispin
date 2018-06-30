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

		private readonly ConditionCollection _collection;

		public ToggleView()
		{
			Tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			Conditions = new List<Condition>();

			_collection = new ConditionCollection(Conditions);
		}

		public void AddCondition(Condition condition, ConditionID parent) => _collection.Add(condition, parent);
		public void RemoveCondition(ConditionID conditionID) => _collection.Remove(conditionID);
	}
}
