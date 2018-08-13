using System;
using System.Collections.Generic;
using System.ComponentModel;
using Crispin.Conditions;
using Crispin.Events;
using Crispin.Views;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Views
{
	public class StatisticViewTests
	{
		private readonly StatisticView _view;
		private readonly UserID _user;
		private readonly DateTime _now;

		public StatisticViewTests()
		{
			_view = new StatisticView();
			_user = UserID.Parse("editor");
			_now = DateTime.Now;
		}

		private void Apply(bool active, DateTime when = default(DateTime)) => _view.Apply(new StatisticReceived(
			_user,
			when == default(DateTime) ? _now : when,
			active: active,
			conditionStates: new Dictionary<ConditionID, bool>()
		));

		[Fact]
		public void Adding_an_inactive_statistic()
		{
			Apply(active: false);

			_view.ShouldSatisfyAllConditions(
				() => _view.TotalQueries.ShouldBe(1),
				() => _view.ActiveQueries.ShouldBe(0),
				() => _view.ActivePercentage.ShouldBe(0),
				() => _view.LastQueried.ShouldBe(_now),
				() => _view.LastInactive.ShouldBe(_now),
				() => _view.LastActive.ShouldBeNull()
			);
		}

		[Fact]
		public void Adding_an_active_statistic()
		{
			Apply(active: true);

			_view.ShouldSatisfyAllConditions(
				() => _view.TotalQueries.ShouldBe(1),
				() => _view.ActiveQueries.ShouldBe(1),
				() => _view.ActivePercentage.ShouldBe(100),
				() => _view.LastQueried.ShouldBe(_now),
				() => _view.LastInactive.ShouldBeNull(),
				() => _view.LastActive.ShouldBe(_now)
			);
		}

		[Fact]
		public void Adding_multiple_adjusts_percentage()
		{
			Apply(active: true, when: _now);
			Apply(active: true, when: _now.AddSeconds(1));
			Apply(active: false, when: _now.AddSeconds(2));
			Apply(active: true, when: _now.AddSeconds(3));

			_view.ShouldSatisfyAllConditions(
				() => _view.TotalQueries.ShouldBe(4),
				() => _view.ActiveQueries.ShouldBe(3),
				() => _view.ActivePercentage.ShouldBe(75),
				() => _view.LastQueried.ShouldBe(_now.AddSeconds(3)),
				() => _view.LastActive.ShouldBe(_now.AddSeconds(3)),
				() => _view.LastInactive.ShouldBe(_now.AddSeconds(2))
			);
		}


		[Fact]
		public void When_events_are_received_out_of_order()
		{
			Apply(active: true, when: _now);
			Apply(active: true, when: _now.AddSeconds(7));
			Apply(active: false, when: _now.AddSeconds(4));
			Apply(active: true, when: _now.AddSeconds(5));

			_view.ShouldSatisfyAllConditions(
				() => _view.TotalQueries.ShouldBe(4),
				() => _view.ActiveQueries.ShouldBe(3),
				() => _view.ActivePercentage.ShouldBe(75),
				() => _view.LastQueried.ShouldBe(_now.AddSeconds(7)),
				() => _view.LastActive.ShouldBe(_now.AddSeconds(7)),
				() => _view.LastInactive.ShouldBe(_now.AddSeconds(4))
			);
		}

		[Fact]
		public void ActiveGraph_groups_and_counts_events_by_second()
		{
			Apply(active: true, when: _now);
			Apply(active: false, when: _now);
			Apply(active: true, when: _now.AddSeconds(1));
			Apply(active: false, when: _now.AddSeconds(1));
			Apply(active: true, when: _now.AddSeconds(1));

			var zero = _now.AddTicks(-(_now.Ticks % TimeSpan.TicksPerSecond));

			_view.QueryGraph.ShouldBe(new Dictionary<DateTime, int>
			{
				{ zero, 2 },
				{ zero.AddSeconds(1), 3 }
			});

			_view.ActiveGraph.ShouldBe(new Dictionary<DateTime, int>
			{
				{ zero, 1 },
				{ zero.AddSeconds(1), 2 }
			});

			_view.InactiveGraph.ShouldBe(new Dictionary<DateTime, int>
			{
				{ zero, 1 },
				{ zero.AddSeconds(1), 1 }
			});
		}

		[Fact]
		public void When_processing_conditions_on_a_toggle()
		{
			_view.Apply(new StatisticReceived(_user, _now, active: true, conditionStates: new Dictionary<ConditionID, bool>
			{
				{ ConditionID.Parse(0), true },
				{ ConditionID.Parse(1), false },
				{ ConditionID.Parse(2), true },
				{ ConditionID.Parse(3), false }
			}));

			_view.Conditions.Keys.ShouldBe(new[]
			{
				ConditionID.Parse(0),
				ConditionID.Parse(1),
				ConditionID.Parse(2),
				ConditionID.Parse(3)
			});
		}
	}
}
