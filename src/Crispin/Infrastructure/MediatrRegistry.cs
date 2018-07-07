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
				a.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
				a.ConnectImplementationsToTypesClosing(typeof(ICancellableAsyncRequestHandler<,>));
				a.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
				a.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
				a.ConnectImplementationsToTypesClosing(typeof(ICancellableAsyncNotificationHandler<>));

				a.ConnectImplementationsToTypesClosing(typeof(IRequestValidator<>));
			});

			For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPreProcessorBehavior<,>));
			For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPostProcessorBehavior<,>));

			For(typeof(IPipelineBehavior<,>)).Add(typeof(TimingBehavior<,>));
			For(typeof(IPipelineBehavior<,>)).Add(typeof(ValidationBehavior<,>));
			For(typeof(IPipelineBehavior<,>)).Add(typeof(StorageBehavior<,>));

			For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
			For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
		}
	}
}
