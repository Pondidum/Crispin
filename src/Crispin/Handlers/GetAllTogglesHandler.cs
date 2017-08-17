using System;
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
					new ToggleView { ID = Guid.NewGuid(), Name = "one", Description = "no", Tags = new []{ "a", "b" }},
					new ToggleView { ID = Guid.NewGuid(), Name = "two", Description = "yes", Tags = new []{ "b" }},
					new ToggleView { ID = Guid.NewGuid(), Name = "three", Description = "unsubscribe" },
				}
			};
		}
	}
}
