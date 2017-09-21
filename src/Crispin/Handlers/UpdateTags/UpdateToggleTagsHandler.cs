using System;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class UpdateToggleTagsHandler :
		IAsyncRequestHandler<AddToggleTagRequest, UpdateToggleTagsResponse>,
		IAsyncRequestHandler<RemoveToggleTagRequest, UpdateToggleTagsResponse>
	{
		private readonly IStorage _storage;

		public UpdateToggleTagsHandler(IStorage storage)
		{
			_storage = storage;
		}

		public Task<UpdateToggleTagsResponse> Handle(AddToggleTagRequest message)
		{
			return ModifyTags(
				message.Locator,
				toggle => toggle.AddTag(message.TagName)
			);
		}

		public Task<UpdateToggleTagsResponse> Handle(RemoveToggleTagRequest message)
		{
			return ModifyTags(
				message.Locator,
				toggle => toggle.RemoveTag(message.TagName)
			);
		}

		private Task<UpdateToggleTagsResponse> ModifyTags(ToggleLocator locator, Action<Toggle> modify)
		{
			using (var session = _storage.BeginSession())
			{
				var toggle = locator.LocateAggregate(session);

				modify(toggle);
				session.Save(toggle);

				return Task.FromResult(new UpdateToggleTagsResponse
				{
					Tags = toggle.Tags.ToArray()
				});
			}
		}
	}
}
