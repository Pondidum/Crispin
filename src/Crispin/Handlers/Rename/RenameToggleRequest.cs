using MediatR;

namespace Crispin.Handlers.Rename
{
	public class RenameToggleRequest : IRequest<RenameToggleResponse>
	{
		public EditorID Editor { get; }
		public ToggleLocator Locator { get; }
		public string Name { get; }

		public RenameToggleRequest(EditorID editor, ToggleLocator locator, string name)
		{
			Editor = editor;
			Locator = locator;
			Name = name;
		}
	}
}
