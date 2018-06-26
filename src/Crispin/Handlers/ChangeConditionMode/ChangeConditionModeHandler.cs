using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.ChangeConditionMode
{
	public class ChangeConditionModeHandler : IAsyncRequestHandler<ChangeConditionModeRequest, ChangeConditionModeResponse>
	{
		private readonly IStorage _storage;

		public ChangeConditionModeHandler(IStorage storage)
		{
			_storage = storage;
		}

		public async Task<ChangeConditionModeResponse> Handle(ChangeConditionModeRequest message)
		{
			using (var session = await _storage.BeginSession())
			{
				var toggle = await message.Locator.LocateAggregate(session);

				if (message.Mode == ConditionModes.All)
					toggle.EnableOnAllConditions(message.Editor);
				else
					toggle.EnableOnAnyCondition(message.Editor);

				await session.Save(toggle);

				return new ChangeConditionModeResponse
				{
					ConditionMode = toggle.ConditionMode,
					Conditions = toggle.Conditions
				};
			}
		}
	}
}
