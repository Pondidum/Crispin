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
				.IsActive(Substitute.For<IToggleReporter>(), Substitute.For<IToggleContext>())
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
						Children = new Condition[]
						{
							new EnabledCondition(),
							new InGroupCondition()
						}
					},
					new DisabledCondition()
				}
			};

			var seenConditions = new List<Type>();
			var reporter = Substitute.For<IToggleReporter>();
			reporter
				.When(r => r.Report(Arg.Any<Condition>(), Arg.Any<bool>()))
				.Do(ci => seenConditions.Add(ci.ArgAt<Condition>(0).GetType()));

			toggle.IsActive(reporter, Substitute.For<IToggleContext>());

			reporter.Received().Report(toggle, false);

			seenConditions.ShouldBe(new[]
			{
				typeof(EnabledCondition),
				typeof(InGroupCondition),
				typeof(AllCondition),
				typeof(DisabledCondition)
			});
		}
	}
}
