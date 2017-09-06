using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class UpdateToggleTagsRequest : IRequest<UpdateToggleTagsResponse>
	{
		public ToggleID ToggleID { get; }
		public IEnumerable<string> Tags { get; set; }

		public UpdateToggleTagsRequest(ToggleID toggleID)
		{
			ToggleID = toggleID;
			Tags = Enumerable.Empty<string>();
		}
	}
}
