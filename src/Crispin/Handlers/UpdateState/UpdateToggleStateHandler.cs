using System;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using MediatR;

namespace Crispin.Handlers.UpdateState
{
	public class UpdateToggleStateHandler : IAsyncRequestHandler<UpdateToggleStateRequest, UpdateToggleStateResponse>
	{
		private readonly IStorage _storage;

		public UpdateToggleStateHandler(IStorage storage)
		{
			_storage = storage;
		}

		public Task<UpdateToggleStateResponse> Handle(UpdateToggleStateRequest message)
		{
			using (var session = _storage.BeginSession())
			{
				var toggle = session.LoadAggregate<Toggle>(message.ToggleID);

				if (toggle == null)
					return Task.FromResult(new UpdateToggleStateResponse());

				if (message.Anonymous.HasValue)
				{
					if (message.Anonymous.Value)
						toggle.SwitchOnByDefault();
					else
						toggle.SwitchOffByDefault();
				}

				foreach (var userState in message.Users)
				{
					if (userState.Value)
						toggle.SwitchOn(userState.Key);
					else
						toggle.SwitchOff(userState.Key);
				}

				session.Save(toggle);
				session.Commit();

				var projection = session.LoadProjection<AllToggles>();
				var view = projection.Toggles.Single(tv => tv.ID == toggle.ID);

				return Task.FromResult(new UpdateToggleStateResponse
				{
					State = view.State
				});
			}
		}
	}
}
