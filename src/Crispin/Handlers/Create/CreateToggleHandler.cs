using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.Create
{
	public class CreateToggleHandler : IAsyncRequestHandler<CreateToggleRequest, CreateTogglesResponse>
	{
		private readonly IStorageSession _session;

		public CreateToggleHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<CreateTogglesResponse> Handle(CreateToggleRequest message)
		{
			var newToggle = Toggle.CreateNew(
				message.Creator,
				message.Name,
				message.Description);

			await _session.Save(newToggle);

			return new CreateTogglesResponse
			{
				Toggle = newToggle.ToView()
			};
		}
	}
}
