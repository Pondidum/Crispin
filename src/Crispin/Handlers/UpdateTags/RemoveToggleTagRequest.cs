using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class RemoveToggleTagRequest : IRequest<UpdateToggleTagsResponse>
	{
		public ToggleLocator Locator { get; }
		public string TagName { get; }

		public RemoveToggleTagRequest(ToggleLocator locator, string tagName)
		{
			Locator = locator;
			TagName = tagName;
		}
	}
}
