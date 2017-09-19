using MediatR;

namespace Crispin.Handlers.UpdateTags
{
	public class RemoveToggleTagRequest : IRequest<UpdateToggleTagsResponse>
	{
		public ToggleID ToggleID { get; }
		public string TagName { get; }

		public RemoveToggleTagRequest(ToggleID toggleID, string tagName)
		{
			ToggleID = toggleID;
			TagName = tagName;
		}
	}
}
