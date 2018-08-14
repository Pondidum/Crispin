using System.Threading;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using MediatR;

namespace Crispin.Handlers.GetAll
{
	public class GetAllTogglesHandler : IRequestHandler<GetAllTogglesRequest, GetAllTogglesResponse>
	{
		private readonly IStorageSession _session;

		public GetAllTogglesHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<GetAllTogglesResponse> Handle(GetAllTogglesRequest message, CancellationToken cancellationToken)
		{
			var projection = await _session.QueryProjection<ToggleView>();

			return new GetAllTogglesResponse
			{
				Toggles = projection
			};
		}
	}
}
