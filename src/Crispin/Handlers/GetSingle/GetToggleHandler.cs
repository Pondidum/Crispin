using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.GetSingle
{
	public class GetToggleHandler : IAsyncRequestHandler<GetToggleRequest, GetToggleResponse>
	{
		private readonly IStorageSession _session;

		public GetToggleHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<GetToggleResponse> Handle(GetToggleRequest message)
		{
			var view = await message.Locator.LocateView(_session);

			return new GetToggleResponse
			{
				Toggle = view
			};
		}
	}
}
