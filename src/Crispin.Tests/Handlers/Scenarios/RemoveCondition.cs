using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Conditions.ConditionTypes;
using Crispin.Handlers.RemoveCondition;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class RemoveCondition : HandlerPipelineTest<RemoveToggleConditionRequest, RemoveToggleConditionResponse>
	{
		private static Dictionary<string, object> Type(string type) => new Dictionary<string, object>
		{
			{ ConditionBuilder.TypeKey, type }
		};

		protected override async Task<RemoveToggleConditionRequest> When()
		{
			await CreateToggle(toggle =>
			{
				toggle.AddCondition(Editor, Type("enabled"));
				toggle.AddCondition(Editor, Type("disabled"));
				toggle.AddCondition(Editor, Type("all"));
			});

			var conditionID = (await Read(ToggleID)).Conditions.OfType<DisabledCondition>().Single().ID;

			return new RemoveToggleConditionRequest(Editor, Locator, conditionID);
		}

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_contains_new_conditions_collection() => Response.Conditions.Count().ShouldBe(2);

		[Fact]
		public async Task The_toggle_is_saved() => (await Read(ToggleID)).Conditions.OfType<DisabledCondition>().ShouldBeEmpty();
	}
}
