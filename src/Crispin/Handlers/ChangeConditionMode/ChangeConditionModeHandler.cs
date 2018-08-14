using System.Threading;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.ChangeConditionMode
{
	public class ChangeConditionModeHandler : IRequestHandler<ChangeConditionModeRequest, ChangeConditionModeResponse>
	{
		private readonly IStorageSession _session;

		public ChangeConditionModeHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<ChangeConditionModeResponse> Handle(ChangeConditionModeRequest message, CancellationToken cancellationToken)
		{
			var toggle = await message.Locator.LocateAggregate(_session);

			if (message.Mode == ConditionModes.All)
				toggle.EnableOnAllConditions(message.Editor);
			else
				toggle.EnableOnAnyCondition(message.Editor);

			await _session.Save(toggle);

			return new ChangeConditionModeResponse
			{
				ConditionMode = toggle.ConditionMode,
				Conditions = toggle.Conditions
			};
		}
	}
}
