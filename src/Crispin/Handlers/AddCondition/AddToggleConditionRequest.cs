using Crispin.Conditions;
using MediatR;

namespace Crispin.Handlers.AddCondition
{
	public class AddToggleConditionRequest : IRequest<AddToggleConditionResponse>
	{
		public ToggleLocator Locator { get; }
		public EditorID Editor { get; }
		public Condition Condition { get; }

		public AddToggleConditionRequest(EditorID editor, ToggleLocator locator, Condition condition)
		{
			Editor = editor;
			Locator = locator;
			Condition = condition;
		}
	}
}
