using System;
using System.Collections.Generic;
using CrispinClient.Contexts;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Contexts
{
	public class ObjectContextTests
	{
		private const string TypeName = nameof(TestTarget);
		private const string GoodGroup = nameof(TestTarget.SomeGroup);
		private const string BadGroup = nameof(TestTarget.SomeBadGroup);
		private const string NullGroup = nameof(TestTarget.NullGroup);
		private const string InvalidGroup = "wat";

		private readonly ObjectContext _context;

		public ObjectContextTests()
		{
			_context = new ObjectContext(new TestTarget
			{
				SomeGroup = new HashSet<string> { "first", "second", "third" },
				SomeBadGroup = new Uri("http://localhost")
			});
		}

		[Theory]
		[InlineData("second", true)]
		[InlineData("fifth", false)]
		public void GroupName_is_mapped_to_a_property(string searchTerm, bool found)
		{
			_context
				.GroupContains(GoodGroup, searchTerm)
				.ShouldBe(found);
		}

		[Fact]
		public void When_a_groupName_doesnt_match_a_property()
		{
			Should
				.Throw<MissingMemberException>(() => _context.GroupContains(InvalidGroup, "is this"))
				.Message.ShouldBe($"Member '{TypeName}.{InvalidGroup}' not found.");
		}

		[Fact]
		public void When_the_property_is_null()
		{
			Should
				.Throw<NullReferenceException>(() => _context.GroupContains(NullGroup, "is this"))
				.Message.ShouldBe($"Member '{TypeName}.{NullGroup}' is null.");
		}

		[Fact]
		public void When_the_property_is_not_enumerable_string()
		{
			Should
				.Throw<InvalidCastException>(() => _context.GroupContains(BadGroup, "is this"))
				.Message.ShouldBe($"Member '{TypeName}.{BadGroup}' does not implement IEnumerable<string>.");
		}


		private class TestTarget
		{
			public HashSet<string> SomeGroup { get; set; }
			public HashSet<string> NullGroup { get; set; }
			public Uri SomeBadGroup { get; set; }
		}
	}
}
