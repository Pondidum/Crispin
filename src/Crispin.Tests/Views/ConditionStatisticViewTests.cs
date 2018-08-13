using System;
using System.Collections.Generic;
using Crispin.Conditions;
using Crispin.Events;
using Crispin.Views;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Views
{
	public class ConditionStatisticViewTests
	{
		private readonly ConditionStatisticView _view;
		private readonly UserID _user;
		private readonly DateTime _now;

		public ConditionStatisticViewTests()
		{
			_view = new ConditionStatisticView();
			_user = UserID.Parse("user");
			_now = DateTime.Now;
		}

		private void Apply(DateTime when, bool active)
		{
			_view.Apply(new StatisticReceived(_user, when, true, new Dictionary<ConditionID, bool>()), active);
		}

		[Fact]
		public void When_processing_an_active_condition()
		{
			Apply(_now, true);

			_view.ShouldSatisfyAllConditions(
				() => _view.LastActive.ShouldBe(_now),
				() => _view.LastInactive.ShouldBeNull(),
				() => _view.QueryGraph.ShouldHaveSingleItem(),
				() => _view.ActiveGraph.ShouldHaveSingleItem(),
				() => _view.InactiveGraph.ShouldBeEmpty()
			);
		}

		[Fact]
		public void When_processing_an_inactive_condition()
		{
			Apply(_now, false);

			_view.ShouldSatisfyAllConditions(
				() => _view.LastActive.ShouldBeNull(),
				() => _view.LastInactive.ShouldBe(_now),
				() => _view.QueryGraph.ShouldHaveSingleItem(),
				() => _view.ActiveGraph.ShouldBeEmpty(),
				() => _view.InactiveGraph.ShouldHaveSingleItem()
			);
		}

		[Fact]
		public void When_processing_multiple_condition_instances_with_different_states()
		{
			Apply(_now, false);
			Apply(_now, true);
			Apply(_now, false);

			Apply(_now.AddSeconds(3), false);
			Apply(_now.AddSeconds(3), true);

			Apply(_now.AddSeconds(5), true);
			Apply(_now.AddSeconds(5), true);
			Apply(_now.AddSeconds(5), true);
			Apply(_now.AddSeconds(5), true);

			_view.ShouldSatisfyAllConditions(
				() => _view.LastActive.ShouldBe(_now.AddSeconds(5)),
				() => _view.LastInactive.ShouldBe(_now.AddSeconds(3))
			);

			var a = Truncate(_now);
			var b = a.AddSeconds(3);
			var c = a.AddSeconds(5);

			_view.QueryGraph.ShouldBe(new Dictionary<DateTime, int>
			{
				{ a, 3 },
				{ b, 2 },
				{ c, 4 }
			});

			_view.ActiveGraph.ShouldBe(new Dictionary<DateTime, int>
			{
				{ a, 1 },
				{ b, 1 },
				{ c, 4 }
			});

			_view.InactiveGraph.ShouldBe(new Dictionary<DateTime, int>
			{
				{ a, 2 },
				{ b, 1 }
			});
		}


		private static DateTime Truncate(DateTime value) => value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
	}
}
