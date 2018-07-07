using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using MediatR;

namespace Crispin.Handlers.GetAll
{
	public class GetAllTogglesHandler : IAsyncRequestHandler<GetAllTogglesRequest, GetAllTogglesResponse>
	{
		private readonly IStorageSession _session;

		public GetAllTogglesHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<GetAllTogglesResponse> Handle(GetAllTogglesRequest message)
		{
			var projection = await _session.LoadProjection<AllTogglesProjection>();

			return new GetAllTogglesResponse
			{
				Toggles = projection.Toggles
			};
		}
	}
}
