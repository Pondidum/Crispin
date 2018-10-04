using System;
using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using CrispinClient.Statistics;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class ToggleTests
	{
		[Fact]
		public void It_can_deserialize_from_the_api()
		{
			var singleToggleJson = @"{
  ""id"": ""57F137CA-4251-4D2D-BD40-EC798854593E"",
  ""conditions"": [
  {
    ""children"": [
      { ""conditionType"": ""Enabled"", ""id"": 1 },
      { ""conditionType"": ""Disabled"", ""id"": 2 }
    ],
    ""conditionType"": ""All"",
    ""id"": 0
  }]
}";

			var toggle = JsonConvert.DeserializeObject<Toggle>(singleToggleJson);

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(Guid.Parse("57F137CA-4251-4D2D-BD40-EC798854593E")),
				() => toggle.Conditions[0].ShouldBeOfType<AllCondition>(),
				() => toggle.Conditions[0].Children.First().ShouldBeOfType<EnabledCondition>(),
				() => toggle.Conditions[0].Children.Last().ShouldBeOfType<DisabledCondition>()
			);
		}

		[Theory]
		[InlineData(ConditionModes.Any, 0, 0, false)]
		[InlineData(ConditionModes.Any, 0, 1, true)]
		[InlineData(ConditionModes.Any, 1, 0, true)]
		[InlineData(ConditionModes.Any, 1, 1, true)]
		//
		[InlineData(ConditionModes.All, 0, 0, false)]
		[InlineData(ConditionModes.All, 0, 1, false)]
		[InlineData(ConditionModes.All, 1, 0, false)]
		[InlineData(ConditionModes.All, 1, 1, true)]
		public void ConditionMode_effects_active_check(ConditionModes mode, int left, int right, bool expected)
		{
			var toggle = new Toggle
			{
				ConditionMode = mode,
				Conditions = new[]
				{
					left == 1 ? new EnabledCondition() : new DisabledCondition() as Condition,
					right == 1 ? new EnabledCondition() : new DisabledCondition() as Condition
				}
			};

			toggle
				.IsActive(Substitute.For<IStatisticsWriter>(), Substitute.For<IToggleContext>())
				.ShouldBe(expected);
		}

		[Fact]
		public void When_writing_statistics()
		{
			var toggle = new Toggle
			{
				Conditions = new Condition[]
				{
					new AllCondition
					{
						ID = 0,
						Children = new Condition[]
						{
							new EnabledCondition { ID = 2 },
							new InGroupCondition { ID = 3 }
						}
					},
					new DisabledCondition { ID = 1 }
				}
			};

			Statistic statistic = null;
			var writer = Substitute.For<IStatisticsWriter>();
			writer.When(w => w.Write(Arg.Any<Statistic>()))
				.Do(ci => statistic = ci.Arg<Statistic>());

			toggle.IsActive(writer, Substitute.For<IToggleContext>());

			statistic.ConditionStates.Keys.ShouldBe(
				new[] { 0, 1, 2, 3 },
				ignoreOrder: true
			);
		}
	}
}
