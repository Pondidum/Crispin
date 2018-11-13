using System.Threading;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.Rename
{
	public class RenameToggleHandler : IRequestHandler<RenameToggleRequest, RenameToggleResponse>
	{
		private readonly IStorageSession _session;

		public RenameToggleHandler(IStorageSession session)
		{
			_session = session;
		}

		public async Task<RenameToggleResponse> Handle(RenameToggleRequest request, CancellationToken cancellationToken)
		{
			var toggle = await request.Locator.LocateAggregate(_session);

			toggle.Rename(request.Editor, request.Name);

			await _session.Save(toggle);

			return new RenameToggleResponse
			{
				ToggleID = toggle.ID,
				Name = toggle.Name
			};
		}
	}
}
