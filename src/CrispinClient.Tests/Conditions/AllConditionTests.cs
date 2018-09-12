using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class AllConditionTests
	{
		private readonly Condition[] _inners;
		private readonly AllCondition _sut;

		public AllConditionTests()
		{
			_inners = Enumerable
				.Range(0, 5)
				.Select(i => Substitute.For<Condition>())
				.ToArray();
			_sut = new AllCondition { Children = _inners };
		}

		public static IEnumerable<object[]> Indexes => Enumerable.Range(0, 5).Select(i => new object[] { i });

		[Theory]
		[MemberData(nameof(Indexes))]
		public void When_any_are_false(int falseIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<IActiveQuery>()).Returns(i != falseIndex);

			_sut.IsMatch(Substitute.For<IActiveQuery>()).ShouldBeFalse();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_false_match_are_not_evaluated(int falseIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<IActiveQuery>()).Returns(i != falseIndex);

			_sut.IsMatch(Substitute.For<IActiveQuery>()).ShouldBeFalse();

			for (int i = falseIndex + 1; i < _inners.Length; i++)
				_inners[i].DidNotReceive().IsMatch(Arg.Any<IActiveQuery>());
		}
	}
}
