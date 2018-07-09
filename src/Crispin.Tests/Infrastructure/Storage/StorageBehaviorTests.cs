using System;
using System.Threading.Tasks;
using Crispin.Infrastructure.Storage;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Storage
{
	public class StorageBehaviorTests
	{
		private readonly StorageBehavior<Request, Response> _behavior;
		private readonly IStorageSession _session;

		public StorageBehaviorTests()
		{
			_session = Substitute.For<IStorageSession>();
			_behavior = new StorageBehavior<Request, Response>(_session);
		}

		[Fact]
		public async Task The_session_is_opened_and_committed_on_a_successful_request()
		{
			await _behavior.Handle(new Request(), () => Task.FromResult(new Response()));

			await _session.Received().Open();
			await _session.Received().Commit();
			await _session.DidNotReceive().Abort();
		}

		[Fact]
		public async Task The_session_is_aborted_if_there_was_an_exception_thrown()
		{
			Should.Throw<ExpectedException>(() =>
				_behavior.Handle(new Request(), () => throw new ExpectedException())
			);

			await _session.Received().Open();
			await _session.DidNotReceive().Commit();
			await _session.Received().Abort();
		}


		private class Request
		{
		}

		private class Response
		{
		}

		private class ExpectedException : Exception
		{
		}
	}
}
