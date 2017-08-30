using System.Collections.Generic;
using MediatR;

namespace Crispin.Handlers.UpdateState
{
	public class UpdateToggleStateRequest : IRequest<UpdateToggleStateResponse>
	{
		public ToggleID ToggleID { get; }
		public bool? Anonymous { get; }
		public Dictionary<string, bool> Groups { get; }
		public Dictionary<string, bool> Users { get; }

		public UpdateToggleStateRequest(ToggleID toggleID, bool? anonymous, Dictionary<string, bool> groups, Dictionary<string, bool> users)
		{
			ToggleID = toggleID;
			Anonymous = anonymous;
			Groups = groups;
			Users = users;
		}
	}
}
