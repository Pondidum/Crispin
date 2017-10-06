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
				var toggle = message.Locator.LocateAggregate(session);

				if (toggle == null)
					return Task.FromResult(new UpdateToggleStateResponse());

				if (message.Default.HasValue)
					toggle.ChangeDefaultState(message.Editor, message.Default.Value);

				foreach (var userState in message.Users)
					toggle.ChangeState(message.Editor,userState.Key, userState.Value);

				foreach (var groupState in message.Groups)
					toggle.ChangeState(message.Editor,groupState.Key, groupState.Value);

				session.Save(toggle);
				session.Commit();

				var projection = session.LoadProjection<AllToggles>();
				var view = projection.Toggles.Single(tv => tv.ID == toggle.ID);

				return Task.FromResult(new UpdateToggleStateResponse
				{
					ToggleID = view.ID,
					State = view.State
				});
			}
		}
	}
}
