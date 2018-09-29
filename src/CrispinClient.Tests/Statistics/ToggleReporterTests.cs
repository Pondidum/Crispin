using System;
using System.Collections.Generic;
using CrispinClient.Conditions;
using CrispinClient.Statistics;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Statistics
{
	public class ToggleStatisticsTests
	{
		private readonly ToggleStatistics _stats;
		private readonly List<Statistic> _written;

		public ToggleStatisticsTests()
		{
			_written = new List<Statistic>();

			var writer = Substitute.For<IStatisticsWriter>();
			writer
				.When(w => w.Write(Arg.Any<Statistic>()))
				.Do(ci => _written.Add(ci.Arg<Statistic>()));

			_stats = new ToggleStatistics(writer);
		}

		[Fact]
		public void When_reporting()
		{
			var toggle = new Toggle
			{
				ID = Guid.NewGuid(),
				Conditions = new Condition[] { new EnabledCondition { ID = 17 } }
			};

			var reporter = _stats.CreateReporter();
			toggle.IsActive(reporter, Substitute.For<IToggleContext>());
			_stats.Complete(reporter);

			var item = _written.ShouldHaveSingleItem();

			item.ShouldSatisfyAllConditions(
				() => item.ToggleID.ShouldBe(toggle.ID),
				() => item.Active.ShouldBe(true),
				() => item.ConditionStates.ShouldHaveSingleItem().ShouldBe(new KeyValuePair<int, bool>(17, true))
			);
		}
	}
}
