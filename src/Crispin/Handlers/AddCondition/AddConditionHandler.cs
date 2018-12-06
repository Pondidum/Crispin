using System.Threading;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.AddCondition
{
	public class AddConditionHandler : IRequestHandler<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		private readonly IStorageSession _session;

		public AddConditionHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<AddToggleConditionResponse> Handle(AddToggleConditionRequest message, CancellationToken cancellationToken)
		{
			var toggle = await message.Locator.LocateAggregate(_session);
			var added = toggle.AddCondition(message.Editor, message.Properties, message.ParentID);

			await _session.Save(toggle);

			return new AddToggleConditionResponse
			{
				ToggleID = toggle.ID,
				AddedConditionID = added,
				Conditions = toggle.Conditions
			};
		}
	}
}
