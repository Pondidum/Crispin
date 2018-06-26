using MediatR;

namespace Crispin.Handlers.ChangeConditionMode
{
	public class ChangeConditionModeRequest : IRequest<ChangeConditionModeResponse>
	{
		public EditorID Editor { get; }
		public ToggleLocator Locator { get; }
		public ConditionModes Mode { get; }

		public ChangeConditionModeRequest(EditorID editor, ToggleLocator locator, ConditionModes mode)
		{
			Editor = editor;
			Locator = locator;
			Mode = mode;
		}
	}
}
