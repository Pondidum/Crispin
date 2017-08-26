using System;
using MediatR;

namespace Crispin.Handlers.GetSingle
{
	public class GetToggleRequest : IRequest<GetToggleResponse>
	{
		public Guid ToggleID { get; }

		public GetToggleRequest(Guid toggleID)
		{
			ToggleID = toggleID;
		}
	}
}
