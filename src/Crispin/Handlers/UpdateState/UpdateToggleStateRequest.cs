using System.Collections.Generic;
using MediatR;

namespace Crispin.Handlers.UpdateState
{
	public class UpdateToggleStateRequest : IRequest<UpdateToggleStateResponse>
	{
		public ToggleLocator Locator { get; }
		public States? Anonymous { get; set; }
		public Dictionary<GroupID, States?> Groups { get; set; }
		public Dictionary<UserID, States?> Users { get; set; }

		public UpdateToggleStateRequest(ToggleLocator locator)
		{
			Locator = locator;
			Groups = new Dictionary<GroupID, States?>();
			Users = new Dictionary<UserID, States?>();
		}
	}
}
