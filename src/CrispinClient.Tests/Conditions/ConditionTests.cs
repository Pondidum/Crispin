using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public abstract class ConditionTests<TCondition> where TCondition : Condition, new()
	{
		public Condition[] ChildConditions { get; }
		public TCondition Sut { get; }
		public Statistic Stats { get; }

		public ConditionTests()
		{
			Stats = new Statistic();
			ChildConditions = Enumerable
				.Range(0, 5)
				.Select(i => Substitute.For<Condition>())
				.ToArray();

			Sut = new TCondition
			{
				ID = 18,
				Children = ChildConditions
			};
		}

		public static IEnumerable<object[]> Indexes => Enumerable
			.Range(0, 5)
			.Select(i => new object[] { i });

		[Fact]
		public void Calling_ismatch_reports_stats()
		{
			Sut.IsMatch(Stats, Substitute.For<IToggleContext>());

			Stats.ConditionStates.Select(states => states.Key).ShouldContain(Sut.ID);
		}
	}
}
