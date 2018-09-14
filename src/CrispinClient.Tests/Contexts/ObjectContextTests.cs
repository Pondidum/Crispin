using System;
using System.Collections.Generic;
using CrispinClient.Contexts;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Contexts
{
	public class ObjectContextTests
	{
		[Theory]
		[InlineData("second", true)]
		[InlineData("fifth", false)]
		public void GroupName_is_mapped_to_a_property(string searchTerm, bool found)
		{
			var context = new ObjectContext(new TestTarget
			{
				SomeGroup = new HashSet<string> { "first", "second", "third" }
			});

			context
				.GroupContains(nameof(TestTarget.SomeGroup), searchTerm)
				.ShouldBe(found);
		}

		[Fact]
		public void When_a_groupName_doesnt_match_a_property()
		{
			var context = new ObjectContext(new TestTarget
			{
				SomeGroup = new HashSet<string> { "first", "second", "third" }
			});

			Should
				.Throw<MissingMemberException>(() => context.GroupContains("wat", "is this"))
				.Message.ShouldBe($"Member '{nameof(TestTarget)}.wat' not found.");
		}

		[Fact]
		public void When_the_property_is_not_enumerable_string()
		{
			var context = new ObjectContext(new TestTarget
			{
				SomeGroup = new HashSet<string> { "first", "second", "third" },
				SomeBadGroup = new Uri("http://localhost")
			});

			Should
				.Throw<InvalidCastException>(() => context.GroupContains(nameof(TestTarget.SomeBadGroup), "is this"))
				.Message.ShouldBe($"Member '{nameof(TestTarget)}.{nameof(TestTarget.SomeBadGroup)}' does not implement IEnumerable<string>.");
		}

		private class TestTarget
		{
			public HashSet<string> SomeGroup { get; set; }
			public Uri SomeBadGroup { get; set; }
		}
	}
}
