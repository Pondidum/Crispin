using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Crispin.Infrastructure.Storage;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.CreateToggle
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
			var response = await _handler.Handle(new CreateToggleRequest(
				creator: EditorID.Parse("?"),
				name: "Test",
				description: "desc"));

			response.Toggle.ShouldNotBeNull();

			await _session.Received().Save(Arg.Is<Toggle>(t => t.ID == response.Toggle.ID));
		}
	}
}
