using MediatR;

namespace Crispin.Handlers.Create
{
	public class CreateToggleRequest : IRequest<CreateTogglesResponse>
	{
		public string UserID { get; }
		public string Name { get; }
		public string Description { get; }

		public CreateToggleRequest(string userID, string name, string description)
		{
			UserID = userID;
			Name = name;
			Description = description;
		}
	}
}