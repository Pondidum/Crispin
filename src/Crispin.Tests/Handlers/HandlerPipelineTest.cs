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
	public abstract class HandlerTest<TRequest, TResponse> : IAsyncLifetime
		where TRequest : IRequest<TResponse>
	{
		protected EditorID Editor { get; }
		protected ToggleID ToggleID { get; private set; }
		protected ToggleLocator Locator => ToggleLocator.Create(ToggleID);

		protected Exception Exception { get; private set; }
		protected TResponse Response { get; private set; }

		private readonly IMediator _mediator;
		private readonly InMemoryStorage _storage;
		private readonly Container _container;

		protected HandlerTest()
		{
			_storage = new InMemoryStorage();
			_storage.RegisterAggregate<ToggleID, Toggle>();
			_storage.RegisterProjection<ToggleView>();

			_container = new Container(_ =>
			{
				_.IncludeRegistry<MediatrRegistry>();

				_.For<IStorage>().Use(_storage);
				_.For<IStorageSession>().Use(c => c.GetInstance<IStorage>().CreateSession()).Scoped();
				_.For<ILoggerFactory>().Use(Substitute.For<ILoggerFactory>());
				_.For(typeof(ILogger<>)).Use(typeof(Logger<>));
			});

			_mediator = _container.GetInstance<IMediator>();

			ToggleID = ToggleID.CreateNew();
			Editor = EditorID.Parse("me");
		}

		public async Task InitializeAsync()
		{
			var request = await When();

			try
			{
				Response = await _mediator.Send(request);
			}
			catch (Exception e)
			{
				Exception = e;
			}
		}

		protected abstract Task<TRequest> When();

		protected async Task CreateToggle(Action<Toggle> setup = null)
		{
			using (var session = _storage.CreateSession())
			{
				var toggle = Toggle.CreateNew(
					Editor,
					"Test Toggle One",
					"Some toggle description goes here");

				setup?.Invoke(toggle);

				await session.Save(toggle);

				ToggleID = toggle.ID;
			}
		}

		protected async Task<Toggle> Read(ToggleID toggleID)
		{
			using (var session = _storage.CreateSession())
				return await session.LoadAggregate<Toggle>(toggleID);
		}

		public Task DisposeAsync()
		{
			_container.Dispose();
			return Task.CompletedTask;
		}
	}
}
