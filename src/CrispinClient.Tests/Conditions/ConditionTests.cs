using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using CrispinClient.Statistics;
using NSubstitute;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public abstract class ConditionTests<TCondition> where TCondition : Condition, new()
	{
		public Condition[] ChildConditions { get; }
		public TCondition Sut { get; }
		public IToggleReporter Reporter { get; }

		public ConditionTests()
		{
			Reporter = Substitute.For<IToggleReporter>();
			ChildConditions = Enumerable
				.Range(0, 5)
				.Select(i => Substitute.For<Condition>())
				.ToArray();
			Sut = new TCondition { Children = ChildConditions };
		}

		public static IEnumerable<object[]> Indexes => Enumerable
			.Range(0, 5)
			.Select(i => new object[] { i });

		[Fact]
		public void Calling_is_match_reports_stats()
		{
			Sut.IsMatch(Reporter, Substitute.For<IToggleContext>());

			Reporter.Received().Report(Sut, Arg.Any<bool>());
		}
	}
}
