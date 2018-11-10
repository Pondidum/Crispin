using System;
using System.Threading.Tasks;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Views;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Lamar;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public abstract class HandlerPipelineTest<TRequest, TResponse> : IAsyncLifetime
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
				_.For<IStorageSession>().Use(c => c.GetInstance<IStorage>().CreateSession()).Scoped();
				_.For<ILoggerFactory>().Use(Substitute.For<ILoggerFactory>());
				_.For(typeof(ILogger<>)).Use(typeof(Logger<>));
			});

			_mediator = container.GetInstance<IMediator>();
		}

		protected async Task<ToggleID> CreateToggle(Action<Toggle> callback = null)
		{
			using (var session = Storage.CreateSession())
			{
				var toggle = Toggle.CreateNew(Editor, "Test Toggle One", "Some toggle description goes here");
				callback?.Invoke(toggle);

				await session.Save(toggle);

				return toggle.ID;
			}
		}

		protected async Task<TResponse> Send(TRequest message) => Response = await _mediator.Send(message);

		protected async Task<Toggle> Read(ToggleID toggleID)
		{
			using (var session = Storage.CreateSession())
			{
				return await session.LoadAggregate<Toggle>(toggleID);
			}
		}

		public virtual Task InitializeAsync() => Task.CompletedTask;
		public virtual Task DisposeAsync() => Task.CompletedTask;
	}
}
