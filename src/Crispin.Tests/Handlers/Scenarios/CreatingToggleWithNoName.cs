using System.Threading.Tasks;
using Crispin.Handlers.Create;
using Crispin.Infrastructure.Validation;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class CreatingToggleWithNoName : HandlerPipelineTest<CreateToggleRequest, CreateTogglesResponse>
	{
		public override async Task InitializeAsync()
		{
			await Send(new CreateToggleRequest(Editor, "", ""));
		}

		[Fact]
		public void It_throws_a_validation_exception_about_the_name() => Exception.ShouldBeOfType<ValidationException>();

		[Fact]
		public void The_exception_message_mentions_the_name_property() => Exception.Message.ShouldContain("Name");
	}
}
