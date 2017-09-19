using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class AddToggleTagRequest : IRequest<UpdateToggleTagsResponse>
	{
		public ToggleID ToggleID { get; }
		public string TagName { get; }

		public AddToggleTagRequest(ToggleID toggleID, string tagName)
		{
			ToggleID = toggleID;
			TagName = tagName;
		}
	}
}
