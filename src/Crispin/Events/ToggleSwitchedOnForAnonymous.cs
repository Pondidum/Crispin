using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOnForAnonymous : Event
	{
		public EditorID Editor { get; }

		public ToggleSwitchedOnForAnonymous(EditorID editor)
		{
			Editor = editor;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned On for Anonymous Users";
		}
	}
}
