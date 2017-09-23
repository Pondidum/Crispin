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

		public Task<GetToggleResponse> Handle(GetToggleRequest message)
		{
			using (var session = _storage.BeginSession())
			{
				var view = message.Locator.LocateView(session);

				return Task.FromResult(new GetToggleResponse
				{
					Toggle = view
				});
			}
		}
	}
}
