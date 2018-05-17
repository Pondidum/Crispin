using Crispin.Infrastructure;
using Crispin.Rules;

namespace Crispin.Events
{
	public class ConditionAdded : Event
	{
		public EditorID Editor { get; }
		public Condition Condition { get; }

		public ConditionAdded(EditorID editor, Condition condition)
		{
			Editor = editor;
			Condition = condition;
		}
	}
}
