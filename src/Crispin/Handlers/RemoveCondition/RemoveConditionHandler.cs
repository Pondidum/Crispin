using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.RemoveCondition
{
	public class RemoveConditionHandler : IAsyncRequestHandler<RemoveToggleConditionRequest, RemoveToggleConditionResponse>
	{
		private readonly IStorageSession _session;

		public RemoveConditionHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<RemoveToggleConditionResponse> Handle(RemoveToggleConditionRequest message)
		{
			var toggle = await message.Locator.LocateAggregate(_session);

			toggle.RemoveCondition(message.Editor, message.ConditionId);

			await _session.Save(toggle);

			return new RemoveToggleConditionResponse
			{
				ConditionMode = toggle.ConditionMode,
				Conditions = toggle.Conditions
			};
		}
	}
}
