using System;
using System.Collections.Generic;
using Crispin.Conditions;
using Crispin.Events;

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

		private readonly ConditionCollection _conditions;
		private readonly ConditionBuilder _conditionBuilder;

		public ToggleView()
		{
			Tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			Conditions = new List<Condition>();

			_conditions = new ConditionCollection(Conditions);
			_conditionBuilder = new ConditionBuilder();
		}

		public void Apply(ToggleCreated e)
		{
			ID = e.NewToggleID;
			Name = e.Name;
			Description = e.Description;
		}

		public void Apply(TagAdded e) => Tags.Add(e.Name);
		public void Apply(TagRemoved e) => Tags.Remove(e.Name);

		public void Apply(EnabledOnAllConditions e) => ConditionMode = ConditionModes.All;
		public void Apply(EnabledOnAnyCondition e) => ConditionMode = ConditionModes.Any;

		public void Apply(ConditionAdded e) => _conditions.Add(
			_conditionBuilder.CreateCondition(e.ConditionID, e.Properties),
			e.ParentConditionID);

		public void Apply(ConditionRemoved e) => _conditions.Remove(e.ConditionID);
	}
}
