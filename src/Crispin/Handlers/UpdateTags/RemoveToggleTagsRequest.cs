using System.Collections.Generic;
using MediatR;

namespace Crispin.Handlers.RemoveTags
{
	public class RemoveToggleTagsRequest : IRequest<RemoveToggleTagsResponse>
	{
		public ToggleID ToggleID { get; }
		public IEnumerable<string> Tags { get; set; }

		public RemoveToggleTagsRequest(ToggleID toggleID)
		{
			ToggleID = toggleID;
		}
	}
}
