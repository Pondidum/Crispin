using Crispin.Conditions;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ConditionAdded : Event
	{
		public EditorID Editor { get; }
		public Condition Condition { get; }
		public int? ParentConditionID { get; }

		public ConditionAdded(EditorID editor, Condition condition)
		{
			Editor = editor;
			Condition = condition;
		}

		public ConditionAdded(EditorID editor, Condition condition, int parentConditionID)
		{
			Editor = editor;
			Condition = condition;
			ParentConditionID = parentConditionID;
		}

		public override string ToString() => ParentConditionID.HasValue
			? $"Added Condition '{Condition}' as a child of Condition {ParentConditionID.Value} to Toggle '{AggregateID}'"
			: $"Added Condition '{Condition}' to Toggle '{AggregateID}'";
	}
}
