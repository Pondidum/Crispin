namespace Crispin.Events
{
	public class TagAdded
	{
		public EditorID Editor { get; }
		public string Name { get; }

		public TagAdded(EditorID editor, string name)
		{
			Editor = editor;
			Name = name;
		}

		public override string ToString()
		{
			return $"Added Tag '{Name}'";
		}
	}
}
