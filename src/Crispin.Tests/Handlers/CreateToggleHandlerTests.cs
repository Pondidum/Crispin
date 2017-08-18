using System;
using System.Threading.Tasks;
using Crispin.Handlers;
using Crispin.Infrastructure.Storage;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class CreateToggleHandlerTests
	{
		private readonly IStorageSession _session;
		private readonly CreateToggleHandler _handler;

		public CreateToggleHandlerTests()
		{
			_session = Substitute.For<IStorageSession>();
			var store = Substitute.For<IStorage>();
			store.BeginSession().Returns(_session);

			_handler = new CreateToggleHandler(store);
		}

		[Fact]
		public async Task When_a_toggle_is_created()
		{
			var response = await _handler.Handle(new CreateToggleRequest("???", "Test", "desc"));

			response.ToggleID.ShouldNotBe(Guid.Empty);

			_session.Received().Save(Arg.Is<Toggle>(t => t.ID == response.ToggleID));
		}
	}
}
