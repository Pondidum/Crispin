using MediatR;

namespace Crispin.Handlers
{
	public class GetToggleByNameRequest : IRequest<GetToggleResponse>
	{
		public string Name { get; }

		public GetToggleByNameRequest(string name)
		{
			Name = name;
		}
	}
}
