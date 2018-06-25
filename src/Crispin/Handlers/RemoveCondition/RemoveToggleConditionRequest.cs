using Crispin.Conditions;
using MediatR;

namespace Crispin.Handlers.RemoveCondition
{
	public class RemoveToggleConditionRequest : IRequest<RemoveToggleConditionResponse>
	{
		public ToggleLocator Locator { get; }
		public EditorID Editor { get; }
		public ConditionID ConditionId { get; }

		public RemoveToggleConditionRequest(EditorID editor, ToggleLocator locator, ConditionID conditionId)
		{
			Editor = editor;
			Locator = locator;
			ConditionId = conditionId;
		}
	}
}
