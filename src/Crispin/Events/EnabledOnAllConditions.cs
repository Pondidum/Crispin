using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class EnabledOnAllConditions : Event
	{
		public EditorID Editor { get; }

		public EnabledOnAllConditions(EditorID editor)
		{
			Editor = editor;
		}

		public override string ToString() => $"Toggle {AggregateID} change to require all conditions to be Enabled to be active";
	}
}
