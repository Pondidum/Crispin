using MediatR;

namespace Crispin.Handlers.GetSingle
{
	public class GetToggleRequest : IRequest<GetToggleResponse>
	{
		public ToggleLocator Locator { get; }

		public GetToggleRequest(ToggleLocator locator)
		{
			Locator = locator;
		}
	}
}
