using System.Threading;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.ChangeDescription
{
	public class ChangeToggleNameHandler : IRequestHandler<ChangeToggleDescriptionRequest, ChangeToggleDescriptionResponse>
	{
		private readonly IStorageSession _session;

		public ChangeToggleNameHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<ChangeToggleDescriptionResponse> Handle(ChangeToggleDescriptionRequest request, CancellationToken cancellationToken)
		{
			var toggle = await request.Locator.LocateAggregate(_session);

			toggle.ChangeDescription(request.Editor, request.Description);

			await _session.Save(toggle);

			return new ChangeToggleDescriptionResponse
			{
				ToggleID = toggle.ID,
				Description = toggle.Description
			};
		}
	}
}
