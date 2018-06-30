using System.Collections.Generic;
using MediatR;

namespace Crispin.Handlers.AddCondition
{
	public class AddToggleConditionRequest : IRequest<AddToggleConditionResponse>
	{
		public ToggleLocator Locator { get; }
		public EditorID Editor { get; }
		public Dictionary<string, object> Properties { get; }

		public AddToggleConditionRequest(EditorID editor, ToggleLocator locator, Dictionary<string, object> properties)
		{
			Editor = editor;
			Locator = locator;
			Properties = properties;
		}
	}
}
