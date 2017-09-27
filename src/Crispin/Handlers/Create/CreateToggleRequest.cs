using MediatR;

namespace Crispin.Handlers.Create
{
	public class CreateToggleRequest : IRequest<CreateTogglesResponse>
	{
		public EditorID Creator { get; }
		public string Name { get; }
		public string Description { get; }

		public CreateToggleRequest(EditorID creator, string name, string description)
		{
			Creator = creator;
			Name = name;
			Description = description;
		}
	}
}