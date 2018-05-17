using Crispin.Infrastructure;
using Crispin.Rules;

namespace Crispin.Events
{
	public class ConditionAdded : Event
	{
		public EditorID Editor { get; }
		public Condition Condition { get; }
		public int ConditionID { get; }

		public ConditionAdded(EditorID editor, Condition condition, int conditionID)
		{
			Editor = editor;
			Condition = condition;
			ConditionID = conditionID;
		}
	}
}
