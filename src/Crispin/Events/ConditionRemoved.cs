using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ConditionRemoved : Event
	{
		public EditorID Editor { get; }
		public int ConditionID { get; }

		public ConditionRemoved(EditorID editor, int conditionID)
		{
			Editor = editor;
			ConditionID = conditionID;
		}
	}
}
