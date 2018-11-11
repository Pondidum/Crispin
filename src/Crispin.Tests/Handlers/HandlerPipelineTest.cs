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
		protected InMemoryStorage Storage { get; }
		protected EditorID Editor { get; }
		protected ToggleID ToggleID { get; }

		protected Exception Exception { get; private set; }
		protected TResponse Response { get; private set; }

		private readonly IMediator _mediator;

		protected HandlerPipelineTest()
		{
			ToggleID = ToggleID.CreateNew();
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

		protected async Task CreateToggle(Action<Toggle> setup = null)
		{
			using (var session = Storage.CreateSession())
			{
				var toggle = Toggle.CreateNew(
					Editor,
					"Test Toggle One",
					"Some toggle description goes here",
					ToggleID);

				setup?.Invoke(toggle);

				await session.Save(toggle);
			}
		}

		protected async Task<TResponse> Send(TRequest message)
		{
			try
			{
				return Response = await _mediator.Send(message);
			}
			catch (Exception e)
			{
				Exception = e;
				return default(TResponse);
			}
		}

		protected async Task<Toggle> Read(ToggleID toggleID)
		{
			using (var session = Storage.CreateSession())
			{
				return await session.LoadAggregate<Toggle>(toggleID);
			}
		}

		public virtual Task InitializeAsync() => CreateToggle();
		public virtual Task DisposeAsync() => Task.CompletedTask;
	}
}
