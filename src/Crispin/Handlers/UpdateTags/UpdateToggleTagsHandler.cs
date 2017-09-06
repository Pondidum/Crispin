using System;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Handlers.RemoveTags;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class UpdateToggleTagsHandler :
		IAsyncRequestHandler<UpdateToggleTagsRequest, UpdateToggleTagsResponse>,
		IAsyncRequestHandler<RemoveToggleTagsRequest, RemoveToggleTagsResponse>
	{
		private readonly IStorage _storage;

		public UpdateToggleTagsHandler(IStorage storage)
		{
			_storage = storage;
		}

		public Task<UpdateToggleTagsResponse> Handle(UpdateToggleTagsRequest message)
		{
			var tags = UpdateToggleTags(message.ToggleID, toggle =>
			{
				foreach (var tag in message.Tags)
					toggle.AddTag(tag);
			});

			return Task.FromResult(new UpdateToggleTagsResponse
			{
				Tags = tags
			});
		}

		public Task<RemoveToggleTagsResponse> Handle(RemoveToggleTagsRequest message)
		{
			var tags = UpdateToggleTags(message.ToggleID, toggle =>
			{
				foreach (var tag in message.Tags)
					toggle.RemoveTag(tag);
			});

			return Task.FromResult(new RemoveToggleTagsResponse
			{
				Tags = tags
			});
		}

		private string[] UpdateToggleTags(ToggleID toggleID, Action<Toggle> process)
		{
			UpdateToggleTagsRequest message;
			using (var session = _storage.BeginSession())
			{
				var toggle = session.LoadAggregate<Toggle>(toggleID);

				process(toggle);

				session.Save(toggle);

				return toggle.Tags.ToArray();
			}
		}
	}
}
