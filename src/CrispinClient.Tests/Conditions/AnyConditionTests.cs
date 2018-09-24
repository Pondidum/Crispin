using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class AnyConditionTests
	{
		private readonly Condition[] _inners;
		private readonly AnyCondition _sut;
		private readonly IToggleReporter _reporter;

		public AnyConditionTests()
		{
			_reporter = Substitute.For<IToggleReporter>();
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
				_inners[i].IsMatch(_reporter, Arg.Any<IToggleContext>()).Returns(i == trueIndex);

			_sut.IsMatch(_reporter, Substitute.For<IToggleContext>()).ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_true_match_are_not_evaluated(int trueIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(_reporter, Arg.Any<IToggleContext>()).Returns(i == trueIndex);

			_sut.IsMatch(_reporter, Substitute.For<IToggleContext>()).ShouldBeTrue();

			for (int i = trueIndex + 1; i < _inners.Length; i++)
				_inners[i].DidNotReceive().IsMatch(_reporter, Arg.Any<IToggleContext>());
		}

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""any"",
				""children"":[
					{""id"": 1, ""conditionType"":""enabled"" },
					{""id"": 2, ""conditionType"":""disabled"" },
				],
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<AnyCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.Children[0].ShouldBeOfType<EnabledCondition>(),
				() => condition.Children[1].ShouldBeOfType<DisabledCondition>()
			);
		}
	}
}
