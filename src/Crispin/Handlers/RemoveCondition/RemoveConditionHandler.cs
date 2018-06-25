using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.RemoveCondition
{
	public class RemoveConditionHandler : IAsyncRequestHandler<RemoveToggleConditionRequest, RemoveToggleConditionResponse>
	{
		private readonly IStorage _storage;

		public RemoveConditionHandler(IStorage storage)
		{
			_storage = storage;
		}

		public async Task<RemoveToggleConditionResponse> Handle(RemoveToggleConditionRequest message)
		{
			using (var session = await _storage.BeginSession())
			{
				var toggle = await message.Locator.LocateAggregate(session);

				toggle.RemoveCondition(message.Editor, message.ConditionId);

				await session.Save(toggle);

				return new RemoveToggleConditionResponse
				{
					ConditionMode = toggle.ConditionMode,
					Conditions = toggle.Conditions
				};
			}
		}
	}
}
