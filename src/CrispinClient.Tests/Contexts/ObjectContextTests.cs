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

		[Theory]
		[InlineData(nameof(TestTarget.SomeGroup), "second", true)]
		[InlineData(nameof(TestTarget.SomeGroup), "fifth", false)]
		[InlineData(nameof(TestTarget.FunctionGroup), "second", true)]
		[InlineData(nameof(TestTarget.FunctionGroup), "fifth", false)]
		[InlineData(nameof(TestTarget.Overloaded), "second", true)]
		[InlineData(nameof(TestTarget.Overloaded), "fifth", false)]
		public void GroupName_is_mapped_to_a_function_or_property(string groupName, string searchTerm, bool found)
		{
			_context
				.GroupContains(groupName, searchTerm)
				.ShouldBe(found);
		}

		[Theory]
		[InlineData(nameof(TestTarget.BadReturn))]
		[InlineData(nameof(TestTarget.BadParameter))]
		[InlineData(nameof(TestTarget.MultipleParameters))]
		public void Matching_methods_should_be_found(string functionName)
		{
			Should
				.Throw<MissingMethodException>(() => _context.GroupContains(functionName, "is this"))
				.Message.ShouldBe($"No method found with a signature 'bool {TypeName}.{functionName}(string term)' (case insensitive).");
		}

		[Fact]
		public void Overloaded_methods_with_no_matching_method()
		{
			var functionName = nameof(TestTarget.BadOverload);

			var message = Should
				.Throw<MissingMethodException>(() => _context.GroupContains(functionName, "is this"))
				.Message;

			message.ShouldSatisfyAllConditions(
				() => message.ShouldStartWith($"No method found with a signature 'bool {TypeName}.{functionName}(string term)' (case insensitive)."),
				() => message.ShouldContain("Found:"),
				() => message.ShouldContain($"* Boolean {TypeName}.{functionName}(Uri one)"),
				() => message.ShouldContain($"* Boolean {TypeName}.{functionName}(String one, String two)"),
				() => message.ShouldContain($"* Void {TypeName}.{functionName}()")
			);
		}

		private class TestTarget
		{
			public HashSet<string> SomeGroup { get; set; }
			public HashSet<string> NullGroup { get; set; }
			public Uri SomeBadGroup { get; set; }

			public bool FunctionGroup(string term) => SomeGroup.Contains(term);
			public Uri BadReturn(string term) => new Uri("http://localhost");
			public bool BadParameter(Uri term) => false;
			public bool MultipleParameters(string one, string two) => false;

			public bool Overloaded(string term) => SomeGroup.Contains(term);
			public bool Overloaded(string one, string two) => true;

			public bool BadOverload(Uri one) => true;
			public bool BadOverload(string one, string two) => true;
			public void BadOverload()
			{
			}
		}
	}
}
