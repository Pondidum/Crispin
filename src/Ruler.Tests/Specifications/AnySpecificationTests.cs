using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Ruler.Specifications;
using Shouldly;
using Xunit;

namespace Ruler.Tests.Specifications
{
	public class AnySpecificationTests
	{
		private readonly ISpecification<string>[] _inners;
		private readonly AnySpecification<string> _sut;

		public AnySpecificationTests()
		{
			_inners = Enumerable
				.Range(0, 5)
				.Select(i => Substitute.For<ISpecification<string>>())
				.ToArray();
			_sut = new AnySpecification<string>(_inners);
		}

		public static IEnumerable<object[]> Indexes => Enumerable.Range(0, 5).Select(i => new object[] { i });

		[Theory]
		[MemberData(nameof(Indexes))]
		public void When_any_are_true(int trueIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<string>()).Returns(i == trueIndex);

			_sut.IsMatch("wat").ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_true_match_are_not_evaluated(int trueIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(Arg.Any<string>()).Returns(i == trueIndex);

			_sut.IsMatch("wat").ShouldBeTrue();

			for (int i = trueIndex + 1; i < _inners.Length; i++)
				_inners[i].DidNotReceive().IsMatch(Arg.Any<string>());
		}
	}
}
