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

				var toRemove = toggle.Tags.Except(message.Tags).ToArray();
				var toAdd = message.Tags.Except(toggle.Tags).ToArray();

				foreach (var tag in toRemove)
					toggle.RemoveTag(tag);

				foreach (var tag in toAdd)
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
