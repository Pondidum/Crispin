using System.Threading.Tasks;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Projections;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using StructureMap;

namespace Crispin.Tests.Handlers
{
	public abstract class HandlerPipelineTest<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		protected TResponse Response { get; set; }
		protected InMemoryStorage Storage { get; }
		protected EditorID Editor { get; }

		private readonly IMediator _mediator;

		protected HandlerPipelineTest()
		{
			Editor = EditorID.Parse("me");
			Storage = new InMemoryStorage();
			Storage.RegisterBuilder(Toggle.LoadFrom);
			Storage.RegisterProjection(new AllTogglesProjection());

			var container = new Container(_ =>
			{
				_.AddRegistry<MediatrRegistry>();

				_.For<IStorage>().Use(Storage);
				_.For<ILoggerFactory>().Use(Substitute.For<ILoggerFactory>());
				_.For(typeof(ILogger<>)).Use(typeof(Logger<>));
			});

			_mediator = container.GetInstance<IMediator>();
		}

		protected async Task<TResponse> Send(TRequest message) => Response = await _mediator.Send(message);
	}
}
