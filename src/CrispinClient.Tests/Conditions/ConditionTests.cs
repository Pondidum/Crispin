using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using NSubstitute;

namespace CrispinClient.Tests.Conditions
{
	public class ConditionTests<TCondition> where TCondition : Condition, new()
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
	}
}
