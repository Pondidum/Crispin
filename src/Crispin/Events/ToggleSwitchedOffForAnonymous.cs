using Crispin.Infrastructure;

namespace Crispin.Events
{
	public class ToggleSwitchedOffForAnonymous : Event
	{
		public EditorID Editor { get; }

		public ToggleSwitchedOffForAnonymous(EditorID editor)
		{
			Editor = editor;
		}

		public override string ToString()
		{
			return $"Toggle '{AggregateID}' turned Off for Anonymous Users";
		}
	}
}
