using Crispin.Infrastructure.Storage;
using Crispin.Infrastructure.Validation;
using MediatR;
using MediatR.Pipeline;
using StructureMap;

namespace Crispin.Infrastructure
{
	public class MediatrRegistry : Registry
	{
		public MediatrRegistry()
		{
			Scan(a =>
			{
				a.AssemblyContainingType<IMediator>();
				a.AssemblyContainingType<Toggle>();
				a.WithDefaultConventions();

				a.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
				a.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
				a.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));

				a.ConnectImplementationsToTypesClosing(typeof(IRequestValidator<>));
			});

			For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPreProcessorBehavior<,>));
			For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPostProcessorBehavior<,>));

			For(typeof(IPipelineBehavior<,>)).Add(typeof(TimingBehavior<,>));
			For(typeof(IPipelineBehavior<,>)).Add(typeof(StorageBehavior<,>));
			For(typeof(IPipelineBehavior<,>)).Add(typeof(ValidationBehavior<,>));

			For<IMediator>().Use<Mediator>().Transient();
			For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
		}
	}
}
