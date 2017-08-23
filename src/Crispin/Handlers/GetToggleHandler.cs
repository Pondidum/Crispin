using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using MediatR;

namespace Crispin.Handlers
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
				var view = session.LoadProjection<AllToggles>();
				var toggle = view
					.Toggles
					.FirstOrDefault(t => t.ID == message.ToggleID);

				return Task.FromResult(new GetToggleResponse
				{
					Toggle = toggle
				});
			}
		}
	}
}
