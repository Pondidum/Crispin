using Crispin.Rest.Toggles;
using MediatR;
using NSubstitute;

namespace Crispin.Rest.Tests.Toggles
{
	public abstract class TogglesControllerTests
	{
		protected IMediator Mediator { get; }
		protected TogglesController Controller { get; }

		protected TogglesControllerTests()
		{
			Mediator = Substitute.For<IMediator>();
			Controller = new TogglesController(Mediator);
		}
	}
}
