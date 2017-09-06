using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class UpdateToggleTagsHandler : IAsyncRequestHandler<UpdateToggleTagsRequest, UpdateToggleTagsResponse>
	{
		private readonly IStorage _storage;

		public UpdateToggleTagsHandler(IStorage storage)
		{
			_storage = storage;
		}

		public Task<UpdateToggleTagsResponse> Handle(UpdateToggleTagsRequest message)
		{
			using (var session = _storage.BeginSession())
			{
				var toggle = session.LoadAggregate<Toggle>(message.ToggleID);

				foreach (var tag in message.Tags)
					toggle.AddTag(tag);

				session.Save(toggle);

				return Task.FromResult(new UpdateToggleTagsResponse
				{
					Tags = toggle.Tags.ToArray()
				});
			}
		}
	}
}
