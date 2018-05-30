using Crispin.Conditions;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ConditionAdded : Event
	{
		public EditorID Editor { get; }
		public Condition Condition { get; }
		public ConditionID ParentConditionID { get; }

		public ConditionAdded(EditorID editor, Condition condition)
		{
			Editor = editor;
			Condition = condition;
		}

		public ConditionAdded(EditorID editor, Condition condition, ConditionID parentConditionID)
		{
			Editor = editor;
			Condition = condition;
			ParentConditionID = parentConditionID;
		}

		public override string ToString() => ParentConditionID != null
			? $"Added Condition '{Condition}' as a child of Condition {ParentConditionID} to Toggle '{AggregateID}'"
			: $"Added Condition '{Condition}' to Toggle '{AggregateID}'";
	}
}
