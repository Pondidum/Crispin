using System;
using MediatR;

namespace Crispin.Handlers.GetSingle
{
	public class GetToggleRequest : IRequest<GetToggleResponse>
	{
		public ToggleID ToggleID { get; }

		public GetToggleRequest(ToggleID toggleID)
		{
			ToggleID = toggleID;
		}
	}
}
