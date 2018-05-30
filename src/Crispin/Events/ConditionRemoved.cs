using Crispin.Conditions;
using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ConditionRemoved : Event
	{
		public EditorID Editor { get; }
		public ConditionID ConditionID { get; }

		public ConditionRemoved(EditorID editor, ConditionID conditionID)
		{
			Editor = editor;
			ConditionID = conditionID;
		}

		public override string ToString() => $"Removed Condition '{ConditionID}' from Toggle '{AggregateID}'";
	}
}
