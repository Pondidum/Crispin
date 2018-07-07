using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Crispin.Infrastructure.Validation;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.CreateToggle
{
	public class CreateTogglePipelineTests : HandlerPipelineTest<CreateToggleRequest, CreateTogglesResponse>
	{
		[Fact]
		public void When_creating_a_toggle_with_no_name()
		{
			Should
				.Throw<ValidationException>(() => Send(new CreateToggleRequest(Editor, "", "")))
				.Message.ShouldContain("Name");
		}

		[Fact]
		public async Task When_creating_a_valid_toggle()
		{
			await Send(new CreateToggleRequest(Editor, "name-one", ""));

			Response.Toggle.ID.ShouldNotBe(ToggleID.Empty);
		}

		[Fact]
		public async Task When_creating_toggle_with_a_name_already_in_use()
		{
			var name = "test-name";
			using (var session = Storage.CreateSession())
				await session.Save(Toggle.CreateNew(Editor, name, "original"));

			Should
				.Throw<ValidationException>(() => Send(new CreateToggleRequest(Editor, name, "repeated name attempt")))
				.Message.ShouldNotBeEmpty();
		}
	}
}
