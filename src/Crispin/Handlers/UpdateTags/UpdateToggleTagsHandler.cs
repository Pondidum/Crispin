﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class UpdateToggleTagsHandler :
		IRequestHandler<AddToggleTagRequest, UpdateToggleTagsResponse>,
		IRequestHandler<RemoveToggleTagRequest, UpdateToggleTagsResponse>
	{
		private readonly IStorageSession _session;

		public UpdateToggleTagsHandler(IStorageSession session)
		{
			_session = session;
		}

		public Task<UpdateToggleTagsResponse> Handle(AddToggleTagRequest message, CancellationToken cancellationToken)
		{
			return ModifyTags(
				message.Locator,
				toggle => toggle.AddTag(message.Editor, message.TagName)
			);
		}

		public Task<UpdateToggleTagsResponse> Handle(RemoveToggleTagRequest message, CancellationToken cancellationToken)
		{
			return ModifyTags(
				message.Locator,
				toggle => toggle.RemoveTag(message.Editor, message.TagName)
			);
		}

		private async Task<UpdateToggleTagsResponse> ModifyTags(ToggleLocator locator, Action<Toggle> modify)
		{
			var toggle = await locator.LocateAggregate(_session);

			modify(toggle);
			await _session.Save(toggle);

			return new UpdateToggleTagsResponse
			{
				ToggleID = toggle.ID,
				Tags = toggle.Tags.ToArray()
			};
		}
	}
}
