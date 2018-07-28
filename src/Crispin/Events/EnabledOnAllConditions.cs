namespace Crispin.Events
{
	public class EnabledOnAllConditions
	{
		public EditorID Editor { get; }

		public EnabledOnAllConditions(EditorID editor)
		{
			Editor = editor;
		}

		public override string ToString() => $"Changed to require all conditions to be Enabled to be active";
	}
}
