namespace Crispin.Events
{
	public class ToggleRenamed
	{
		public EditorID Editor { get; }
		public string NewName { get; }

		public ToggleRenamed(EditorID editor, string newName)
		{
			Editor = editor;
			NewName = newName;
		}

		public override string ToString()
		{
			return $"Renamed to '{NewName}'";
		}
	}
}
