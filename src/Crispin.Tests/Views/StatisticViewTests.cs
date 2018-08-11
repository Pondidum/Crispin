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

		public StatisticViewTests()
		{
			_view = new StatisticView();
			_user = UserID.Parse("editor");
		}

		private void Apply(bool active, DateTime when = default(DateTime)) => _view.Apply(new StatisticReceived(
			_user,
			when == default(DateTime) ? DateTime.Now : when,
			active: active,
			conditionStates: new Dictionary<ConditionID, bool>()
		));

		[Fact]
		public void Adding_an_inactive_statistic_increases_active_count()
		{
			Apply(active: false);

			_view.ShouldSatisfyAllConditions(
				() => _view.TotalQueries.ShouldBe(1),
				() => _view.ActiveQueries.ShouldBe(0),
				() => _view.ActivePercentage.ShouldBe(0)
			);
		}

		[Fact]
		public void Adding_an_active_statistic_doesnt_affect_active_count()
		{
			Apply(active: true);

			_view.ShouldSatisfyAllConditions(
				() => _view.TotalQueries.ShouldBe(1),
				() => _view.ActiveQueries.ShouldBe(1),
				() => _view.ActivePercentage.ShouldBe(100)
			);
		}

		[Fact]
		public void Adding_multiple_adjusts_percentage()
		{
			Apply(active: true);
			Apply(active: true);
			Apply(active: false);
			Apply(active: true);

			_view.ShouldSatisfyAllConditions(
				() => _view.TotalQueries.ShouldBe(4),
				() => _view.ActiveQueries.ShouldBe(3),
				() => _view.ActivePercentage.ShouldBe(75)
			);
		}
	}
}
