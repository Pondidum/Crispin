using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Crispin.Infrastructure.Validation;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class CreatingToggleWithAnInUseName : HandlerTest<CreateToggleRequest, CreateTogglesResponse>
	{
		protected override async Task<CreateToggleRequest> When()
		{
			await CreateToggle();
			var existing = await Read(ToggleID);

			return new CreateToggleRequest(Editor, existing.Name, "repeated name attempt");
		}

		[Fact]
		public void It_throws_a_validation_exception_about_the_name() => Exception.ShouldBeOfType<ValidationException>();

		[Fact]
		public void The_exception_message_mentions_the_name_property() => Exception.Message.ShouldContain("Name");
	}
}
