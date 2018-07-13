using System;
using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Crispin.Handlers.GetAll;
using Crispin.Infrastructure;
using Crispin.Infrastructure.Storage;
using Crispin.Infrastructure.Validation;
using Crispin.Rest.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using StructureMap;
using Xunit;

namespace Crispin.Rest.Tests
{
	public class ContainerTests
	{
		private readonly Container _container;

		public ContainerTests()
		{
			_container = new Container(_ =>
			{
				_.AddRegistry<CrispinRestRegistry>();
				_.AddRegistry<MediatrRegistry>();

				_.For(typeof(ILogger<>)).Use(typeof(FakeLogger<>));
				_.For<IStorage>().Use(StorageBuilder.Configure(new InMemoryStorage()));
			});
		}

		[Fact]
		public void When_resolving_a_validator()
		{
			_container
				.GetInstance<ValidationBehavior<CreateToggleRequest, CreateTogglesResponse>>()
				.ShouldNotBeNull();
		}

		[Fact]
		public async Task When_resolving_a_validator_which_doesnt_exist()
		{
			var mediator = _container.GetInstance<IMediator>();
			var response = await mediator.Send(new GetAllTogglesRequest());

			response.ShouldBeOfType<GetAllTogglesResponse>();
		}

		private class FakeLogger<T> : ILogger<T>
		{
			public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
			{
			}

			public bool IsEnabled(LogLevel logLevel)
			{
				return true;
			}

			public IDisposable BeginScope<TState>(TState state)
			{
				return Substitute.For<IDisposable>();
			}
		}
	}
}
