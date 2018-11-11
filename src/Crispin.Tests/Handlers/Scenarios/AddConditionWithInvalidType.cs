using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers.AddCondition;
using Crispin.Infrastructure.Validation;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class AddConditionWithInvalidType : HandlerTest<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		protected override async Task<AddToggleConditionRequest> When()
		{
			await CreateToggle();

			return new AddToggleConditionRequest(Editor, Locator, new Dictionary<string, object>
			{
				{ "type", "waaaaaaaaaaaaaat" }
			});
		}

		[Fact]
		public void It_throws_a_validation_exception() => Exception.ShouldBeOfType<ValidationException>();
	}
}
