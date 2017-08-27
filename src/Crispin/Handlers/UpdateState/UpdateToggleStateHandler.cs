using System;
using System.Collections.Generic;
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
						toggle.SwitchOn(UserID.Empty, GroupID.Empty);
					else
						toggle.SwitchOff(UserID.Empty, GroupID.Empty);
				}

				session.Save(toggle);
				session.Commit();

				return Task.FromResult(new UpdateToggleStateResponse());
			}
		}
	}

	public class UpdateToggleStateRequest : IRequest<UpdateToggleStateResponse>
	{
		public ToggleID ToggleID { get; }
		public bool? Anonymous { get; }
		public Dictionary<string, bool> Groups { get; }
		public Dictionary<string, bool> Users { get; }

		public UpdateToggleStateRequest(ToggleID toggleID, bool? anonymous, Dictionary<string, bool> groups, Dictionary<string, bool> users)
		{
			ToggleID = toggleID;
			Anonymous = anonymous;
			Groups = groups;
			Users = users;
		}
	}

	public class UpdateToggleStateResponse
	{
		//public StateView State { get; set; }
	}
}
