using System;
using System.Collections.Generic;
using Crispin.Infrastructure.Statistics;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Infrastructure.Statistics
{
	public class ExtensionsTests
	{
		public static IEnumerable<object[]> StringRenderingCases => new List<object[]>
		{
			new object[] { "with.no.replacements", Array.Empty<object>(), "with.no.replacements" },
			new object[] { "with.{one}.replacement", new[] { "test" }, "with.test.replacement" },
			new object[] { "with.{first}.{second}.replacement", new[] { "one", "two" }, "with.one.two.replacement" },
			new object[] { "with.{first}.{second}.replacement", new[] { "one", "two", "three" }, "with.one.two.replacement" }
		};

		[Theory]
		[MemberData(nameof(StringRenderingCases))]
		public void When_rendering_a_valid_string_and_parameters(string format, object[] parameters, string expected)
		{
			format
				.Render(parameters)
				.ShouldBe(expected);
		}

		[Fact]
		public void When_there_are_too_few_parameters()
		{
			Should.Throw<FormatException>(() => "some.{test}{value}{replacement}".Render("one"));
		}

		public static IEnumerable<object[]> TemplateCases => new List<object[]>
		{
			new object[]
			{
				"with.no.replacements",
				Array.Empty<object>(),
				new Dictionary<string, object>()
			},
			new object[]
			{
				"with.{one}.replacement",
				new[] { "test" },
				new Dictionary<string, object> { { "one", "test" } }
			},
			new object[]
			{
				"with.{first}.{second}.replacement",
				new[] { "one", "two" },
				new Dictionary<string, object> { { "first", "one" }, { "second", "two" } }
			}
		};

		[Theory]
		[MemberData(nameof(TemplateCases))]
		public void When_building_property_map(string format, object[] parameters, Dictionary<string, object> expected)
		{
			format.BuildPropertyMap(parameters).ShouldBe(expected);
		}
	}
}
