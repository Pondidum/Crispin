using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.AddCondition
{
	public class AddConditionHandler : IAsyncRequestHandler<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		private readonly IStorage _storage;

		public AddConditionHandler(IStorage storage)
		{
			_storage = storage;
		}

		public async Task<AddToggleConditionResponse> Handle(AddToggleConditionRequest message)
		{
			using (var session = await _storage.BeginSession())
			{
				var toggle = await message.Locator.LocateAggregate(session);

				toggle.AddCondition(message.Editor, message.Condition);

				await session.Save(toggle);

				return new AddToggleConditionResponse
				{
					ToggleID = toggle.ID,
					Condition = message.Condition
				};
			}
		}
	}
}
