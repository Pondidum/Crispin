using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Conditions.ConditionTypes;
using Crispin.Handlers.AddCondition;
using Crispin.Infrastructure.Validation;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.AddCondition
{
	public class AddConditionPipelineTests : HandlerPipelineTest<AddToggleConditionRequest, AddToggleConditionResponse>
	{
		private static readonly ToggleLocator Locator = ToggleLocator.Create("wat");
		private readonly Dictionary<string, object> _props;

		public AddConditionPipelineTests()
		{
			_props = new Dictionary<string, object>();
		}

		[Fact]
		public void When_adding_a_condition_with_no_props()
		{
			Should
				.Throw<ValidationException>(() => Send(new AddToggleConditionRequest(Editor, Locator, new Dictionary<string, object>())))
				.Message.ShouldContain("Type was not specified");
		}

		[Fact]
		public void When_adding_a_condition_which_doesnt_exist()
		{
			_props["type"] = "whhhaaaaat";

			Should.Throw<ValidationException>(
				() => Send(new AddToggleConditionRequest(Editor, Locator, _props))
			);
		}

		[Fact]
		public async Task When_adding_a_condition()
		{
			var toggle = Toggle.CreateNew(Editor, "Toggle", "original");
			using (var session = await Storage.BeginSession())
				await session.Save(toggle);

			_props["type"] = "enabled";

			var response = await Send(new AddToggleConditionRequest(Editor, ToggleLocator.Create(toggle.ID), _props));

			response
				.Conditions
				.ShouldHaveSingleItem()
				.ShouldBeOfType<EnabledCondition>();
		}
	}
}
