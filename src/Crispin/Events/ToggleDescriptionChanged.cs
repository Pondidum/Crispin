namespace Crispin.Events
{
	public class ToggleDescriptionChanged
	{
		public EditorID Editor { get; }
		public string NewDescription { get; }

		public ToggleDescriptionChanged(EditorID editor, string newDescription)
		{
			Editor = editor;
			NewDescription = newDescription;
		}

		public override string ToString()
		{
			return $"Description changed to '{NewDescription}'";
		}
	}
}
