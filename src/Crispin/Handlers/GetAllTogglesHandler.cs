using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Projections;
using MediatR;

namespace Crispin.Handlers
{
	public class GetAllTogglesHandler : IAsyncRequestHandler<GetAllTogglesRequest, GetAllTogglesResponse>
	{
		public async Task<GetAllTogglesResponse> Handle(GetAllTogglesRequest message)
		{
			return new GetAllTogglesResponse
			{
				Toggles = new []
				{
					new ToggleView { ID = Guid.NewGuid(), Name = "one", Description = "no", Tags = new HashSet<string> { "a", "b" }},
					new ToggleView { ID = Guid.NewGuid(), Name = "two", Description = "yes", Tags = new HashSet<string> { "b" }},
					new ToggleView { ID = Guid.NewGuid(), Name = "three", Description = "unsubscribe" },
				}
			};
		}
	}
}
