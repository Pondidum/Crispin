using System;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using MediatR;

namespace Crispin.Handlers
{
	public class GetToggleHandler : IAsyncRequestHandler<GetToggleRequest, GetToggleResponse>, IAsyncRequestHandler<GetToggleByNameRequest, GetToggleResponse>
	{
		private readonly IStorage _storage;

		public GetToggleHandler(IStorage storage)
		{
			_storage = storage;
		}

		public Task<GetToggleResponse> Handle(GetToggleRequest message)
		{
			return FindToggle(t => t.ID == message.ToggleID);
		}

		public Task<GetToggleResponse> Handle(GetToggleByNameRequest message)
		{
			return FindToggle(t => string.Equals(t.Name, message.Name, StringComparison.OrdinalIgnoreCase));
		}

		private Task<GetToggleResponse> FindToggle(Func<ToggleView, bool> filter)
		{
			using (var session = _storage.BeginSession())
			{
				var view = session.LoadProjection<AllToggles>();
				var toggle = view
					.Toggles
					.FirstOrDefault(filter);

				return Task.FromResult(new GetToggleResponse
				{
					Toggle = toggle
				});
			}
		}
	}
}
