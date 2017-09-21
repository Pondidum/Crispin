using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class AddToggleTagRequest : IRequest<UpdateToggleTagsResponse>
	{
		public ToggleLocator Locator { get; }
		public string TagName { get; }

		public AddToggleTagRequest(ToggleLocator locator, string tagName)
		{
			Locator = locator;
			TagName = tagName;
		}
	}
}
