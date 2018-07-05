using System.Collections.Generic;
using Crispin.Conditions;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ConditionAdded : Event
	{
		public EditorID Editor { get; }
		public ConditionID ConditionID { get; }
		public ConditionID ParentConditionID { get; }
		public Dictionary<string, object> Properties { get; }

		public ConditionAdded(EditorID editor, ConditionID conditionID, ConditionID parentConditionID, Dictionary<string, object> conditionProperties)
		{
			Editor = editor;
			ConditionID = conditionID;
			ParentConditionID = parentConditionID;
			Properties = conditionProperties;
		}

		public override string ToString() => ParentConditionID != null
			? $"Added Condition '{Properties[ConditionBuilder.TypeKey]}' as a child of Condition {ParentConditionID} to Toggle '{AggregateID}'"
			: $"Added Condition '{Properties[ConditionBuilder.TypeKey]}' to Toggle '{AggregateID}'";
	}
}
