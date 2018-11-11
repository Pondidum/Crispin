using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers.AddCondition;
using Crispin.Infrastructure.Validation;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class AddConditionWithInvalidType : HandlerPipelineTest<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		public override async Task InitializeAsync()
		{
			await CreateToggle();

			await Send(new AddToggleConditionRequest(Editor, ToggleLocator.Create(ToggleID), new Dictionary<string, object>
			{
				{ "type", "waaaaaaaaaaaaaat" }
			}));
		}

		[Fact]
		public void It_throws_a_validation_exception() => Exception.ShouldBeOfType<ValidationException>();
	}
}
