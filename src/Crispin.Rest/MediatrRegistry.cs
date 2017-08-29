using Crispin.Infrastructure;
using MediatR;
using MediatR.Pipeline;
using StructureMap;

namespace Crispin.Rest
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
			});

			For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPreProcessorBehavior<,>));
			For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPostProcessorBehavior<,>));

			For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
			For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
		}
	}
}
