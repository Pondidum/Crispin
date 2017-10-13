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
				toggle => toggle.AddTag(message.Editor, message.TagName)
			);
		}

		public Task<UpdateToggleTagsResponse> Handle(RemoveToggleTagRequest message)
		{
			return ModifyTags(
				message.Locator,
				toggle => toggle.RemoveTag(message.Editor, message.TagName)
			);
		}

		private async Task<UpdateToggleTagsResponse> ModifyTags(ToggleLocator locator, Action<Toggle> modify)
		{
			using (var session = await _storage.BeginSession())
			{
				var toggle = await locator.LocateAggregate(session);

				modify(toggle);
				session.Save(toggle);

				return new UpdateToggleTagsResponse
				{
					Tags = toggle.Tags.ToArray()
				};
			}
		}
	}
}
