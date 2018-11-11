using MediatR;

namespace Crispin.Handlers.ChangeDescription
{
	public class ChangeToggleDescriptionRequest : IRequest<ChangeToggleDescriptionResponse>
	{
		public EditorID Editor { get; }
		public ToggleLocator Locator { get; }
		public string Description { get; }

		public ChangeToggleDescriptionRequest(EditorID editor, ToggleLocator locator, string description)
		{
			Editor = editor;
			Locator = locator;
			Description = description;
		}
	}
}
