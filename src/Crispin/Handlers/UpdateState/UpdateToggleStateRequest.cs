using System.Collections.Generic;
using MediatR;

namespace Crispin.Handlers.UpdateState
{
	public class UpdateToggleStateRequest : IRequest<UpdateToggleStateResponse>
	{
		public ToggleID ToggleID { get; }
		public bool? Anonymous { get; set; }
		public Dictionary<GroupID, bool> Groups { get; set; }
		public Dictionary<UserID, bool> Users { get; set; }

		public UpdateToggleStateRequest(ToggleID toggleID)
		{
			ToggleID = toggleID;
			Groups = new Dictionary<GroupID, bool>();
			Users = new Dictionary<UserID, bool>();
		}
	}
}
