using Crispin.Infrastructure;
using Crispin.Rules;

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
	}
}
