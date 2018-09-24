using CrispinClient.Conditions;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Conditions
{
	public class NotConditionTests
	{
		private readonly Condition _inner;
		private readonly NotCondition _sut;
		private readonly IToggleReporter _reporter;

		public NotConditionTests()
		{
			_reporter = Substitute.For<IToggleReporter>();
			_inner = Substitute.For<Condition>();
			_sut = new NotCondition { Children = new[] { _inner } };
		}

		[Fact]
		public void When_the_inner_spec_is_true()
		{
			_inner.IsMatch(_reporter, Arg.Any<IToggleContext>()).Returns(true);

			_sut.IsMatch(_reporter, Substitute.For<IToggleContext>()).ShouldBeFalse();
		}

		[Fact]
		public void When_the_inner_spec_is_false()
		{
			_inner.IsMatch(_reporter, Arg.Any<IToggleContext>()).Returns(false);

			_sut.IsMatch(_reporter, Substitute.For<IToggleContext>()).ShouldBeTrue();
		}

		[Fact]
		public void When_deserializing()
		{
			var conditionJson = @"{
				""id"": 17,
				""conditionType"": ""not"",
				""children"":[
					{""id"": 1, ""conditionType"":""enabled"" }
				]
			}";

			var condition = JsonConvert
				.DeserializeObject<Condition>(conditionJson)
				.ShouldBeOfType<NotCondition>();

			condition.ShouldSatisfyAllConditions(
				() => condition.ID.ShouldBe(17),
				() => condition.Children.ShouldHaveSingleItem().ShouldBeOfType<EnabledCondition>()
			);
		}
	}
}
