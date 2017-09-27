using System.Collections.Generic;
using MediatR;

namespace Crispin.Handlers.UpdateState
{
	public class UpdateToggleStateRequest : IRequest<UpdateToggleStateResponse>
	{
		public EditorID Editor { get; }
		public ToggleLocator Locator { get; }
		public States? Anonymous { get; set; }
		public Dictionary<GroupID, States?> Groups { get; set; }
		public Dictionary<UserID, States?> Users { get; set; }

		public UpdateToggleStateRequest(EditorID editor, ToggleLocator locator)
		{
			Editor = editor;
			Locator = locator;
			Groups = new Dictionary<GroupID, States?>();
			Users = new Dictionary<UserID, States?>();
		}
	}
}
