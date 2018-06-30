using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.AddCondition
{
	public class AddConditionHandler : IAsyncRequestHandler<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		private readonly IStorage _storage;
		private readonly IConditionBuilder _builder;

		public AddConditionHandler(IStorage storage, IConditionBuilder builder)
		{
			_storage = storage;
			_builder = builder;
		}

		public async Task<AddToggleConditionResponse> Handle(AddToggleConditionRequest message)
		{
			var condition = _builder.CreateCondition(message.Properties);

			using (var session = await _storage.BeginSession())
			{
				var toggle = await message.Locator.LocateAggregate(session);

				toggle.AddCondition(message.Editor, condition);

				await session.Save(toggle);

				return new AddToggleConditionResponse
				{
					ConditionMode = toggle.ConditionMode,
					Conditions = toggle.Conditions
				};
			}
		}
	}
}
