using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.AddCondition
{
	public class AddConditionHandler : IAsyncRequestHandler<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		private readonly IStorageSession _session;

		public AddConditionHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<AddToggleConditionResponse> Handle(AddToggleConditionRequest message)
		{
			var toggle = await message.Locator.LocateAggregate(_session);
			var added = toggle.AddCondition(message.Editor, message.Properties);

			await _session.Save(toggle);

			return new AddToggleConditionResponse
			{
				ToggleID = toggle.ID,
				Condition = toggle.Condition(added)
			};
		}
	}
}
