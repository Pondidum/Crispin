using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.GetSingle
{
	public class GetToggleHandler : IAsyncRequestHandler<GetToggleRequest, GetToggleResponse>
	{
		private readonly IStorage _storage;

		public GetToggleHandler(IStorage storage)
		{
			_storage = storage;
		}

		public async Task<GetToggleResponse> Handle(GetToggleRequest message)
		{
			using (var session = await _storage.BeginSession())
			{
				var view = await message.Locator.LocateView(session);

				return new GetToggleResponse
				{
					Toggle = view
				};
			}
		}
	}
}
