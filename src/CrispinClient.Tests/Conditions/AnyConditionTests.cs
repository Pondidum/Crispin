using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class AnyConditionTests
	{
		private readonly Condition[] _inners;
		private readonly AnyCondition _sut;

		public AnyConditionTests()
		{
			_inners = Enumerable
				.Range(0, 5)
				.Select(i => Substitute.For<Condition>())
				.ToArray();
			_sut = new AnyCondition { Children = _inners };
		}

		public static IEnumerable<object[]> Indexes => Enumerable.Range(0, 5).Select(i => new object[] { i });

		[Theory]
		[MemberData(nameof(Indexes))]
		public void When_any_are_true(int trueIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<IActiveQuery>()).Returns(i == trueIndex);

			_sut.IsMatch(Substitute.For<IActiveQuery>()).ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_true_match_are_not_evaluated(int trueIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<IActiveQuery>()).Returns(i == trueIndex);

			_sut.IsMatch(Substitute.For<IActiveQuery>()).ShouldBeTrue();

			for (int i = trueIndex + 1; i < _inners.Length; i++)
				_inners[i].DidNotReceive().IsMatch(Arg.Any<IActiveQuery>());
		}
	}
}
