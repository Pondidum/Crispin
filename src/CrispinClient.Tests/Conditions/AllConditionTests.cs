using System.Collections.Generic;
using System.Linq;
using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class AllConditionTests
	{
		private readonly Condition[] _inners;
		private readonly AllCondition _sut;
		private readonly IToggleReporter _reporter;

		public AllConditionTests()
		{
			_reporter = Substitute.For<IToggleReporter>();
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
				_inners[i].IsMatch(_reporter, Arg.Any<IToggleContext>()).Returns(i != falseIndex);

			_sut.IsMatch(_reporter, Substitute.For<IToggleContext>()).ShouldBeFalse();
		}

		[Theory]
		[MemberData(nameof(Indexes))]
		public void Children_after_first_false_match_are_not_evaluated(int falseIndex)
		{
			for (int i = 0; i < _inners.Length; i++)
				_inners[i].IsMatch(_reporter, Arg.Any<IToggleContext>()).Returns(i != falseIndex);

			_sut.IsMatch(_reporter, Substitute.For<IToggleContext>()).ShouldBeFalse();

			for (int i = falseIndex + 1; i < _inners.Length; i++)
				_inners[i].DidNotReceive().IsMatch(_reporter, Arg.Any<IToggleContext>());
		}

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""all"",
				""children"":[
					{""id"": 1, ""conditionType"":""enabled"" },
					{""id"": 2, ""conditionType"":""disabled"" },
				],
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<AllCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.Children[0].ShouldBeOfType<EnabledCondition>(),
				() => condition.Children[1].ShouldBeOfType<DisabledCondition>()
			);
		}
	}
}
