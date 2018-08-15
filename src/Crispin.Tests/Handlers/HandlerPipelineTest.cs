using System.Threading.Tasks;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Lamar;

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
			Storage.RegisterAggregate<ToggleID, Toggle>();
			Storage.RegisterProjection<ToggleView>();

			var container = new Container(_ =>
			{
				_.IncludeRegistry<MediatrRegistry>();

				_.For<IStorage>().Use(Storage);
				_.For<IStorageSession>().Use(c => c.GetInstance<IStorage>().CreateSession());
				_.For<ILoggerFactory>().Use(Substitute.For<ILoggerFactory>());
				_.For(typeof(ILogger<>)).Use(typeof(Logger<>));
			});

			_mediator = container.GetInstance<IMediator>();
		}

		protected async Task<TResponse> Send(TRequest message) => Response = await _mediator.Send(message);
	}
}
