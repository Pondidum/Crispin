using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Ruler.Specifications;
using Shouldly;
using Xunit;

namespace Ruler.Tests.Specifications
{
	public class AllSpecificationTests
	{
		private readonly ISpecification<string>[] _inners;
		private readonly AllSpecification<string> _sut;

		public AllSpecificationTests()
		{
			_inners = Enumerable
				.Range(0, 5)
				.Select(i => Substitute.For<ISpecification<string>>())
				.ToArray();
			_sut = new AllSpecification<string>(_inners);
		}

		public static IEnumerable<object[]> Indexes => Enumerable.Range(0, 5).Select(i => new object[] { i });

		[Theory]
		[MemberData(nameof(Indexes))]
		public void When_any_are_false(int falseIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<string>()).Returns(i != falseIndex);

			_sut.IsMatch("wat").ShouldBeFalse();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_false_match_are_not_evaluated(int falseIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<string>()).Returns(i != falseIndex);

			_sut.IsMatch("wat").ShouldBeFalse();

			for (int i = falseIndex + 1; i < _inners.Length; i++)
				_inners[i].DidNotReceive().IsMatch(Arg.Any<string>());
		}
	}
}
