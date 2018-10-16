using System;
using System.Collections.Generic;
using CrispinClient.Contexts;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests.Contexts
{
	public class ObjectContextTests
	{
		private const string TargetName = nameof(TestTarget);

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
		public void When_querying_a_group_and_the_property_is_null()
		{
			var groupName = nameof(TestTarget.NullGroup);

			Should
				.Throw<NullReferenceException>(() => _context.GroupContains(groupName, "is this"))
				.Message.ShouldBe($"Member '{TargetName}.{groupName}' is null.");
		}

		[Theory]
		[InlineData(nameof(TestTarget.SomeGroup), "second", true)]
		[InlineData(nameof(TestTarget.SomeGroup), "fifth", false)]
		[InlineData(nameof(TestTarget.FunctionGroup), "second", true)]
		[InlineData(nameof(TestTarget.FunctionGroup), "fifth", false)]
		[InlineData(nameof(TestTarget.Overloaded), "second", true)]
		[InlineData(nameof(TestTarget.Overloaded), "fifth", false)]
		public void When_querying_a_group_and_the_groupName_is_mapped_to_a_function_or_property(string groupName, string searchTerm, bool found)
		{
			_context
				.GroupContains(groupName, searchTerm)
				.ShouldBe(found);
		}

		[Theory]
		[InlineData(nameof(TestTarget.BadReturn))]
		[InlineData(nameof(TestTarget.BadParameter))]
		[InlineData(nameof(TestTarget.MultipleParameters))]
		[InlineData(nameof(TestTarget.BadOverload))]
		[InlineData(nameof(TestTarget.SomeBadGroup))]
		[InlineData("wat")]
		public void When_querying_a_group_matching_methods_should_be_found(string groupName)
		{
			var message = Should
				.Throw<MissingMethodException>(() => _context.GroupContains(groupName, "is this"))
				.Message;

			message.ShouldSatisfyAllConditions(
				() => message.ShouldStartWith($"No method or property found to query for group '{groupName}'."),
				() => message.ShouldContain($"* 'bool {TargetName}.{groupName}(string term)'"),
				() => message.ShouldContain($"* 'IEnumerable<string> {TargetName}.{groupName} {{ get; }}'")
			);
		}

		[Fact]
		public void When_querying_current_user_from_a_property()
		{
			var context = new ObjectContext(new TestUserProperty());

			context.GetCurrentUser().ShouldBe("Dave");
		}

		[Fact]
		public void When_querying_current_user_from_a_method()
		{
			var context = new ObjectContext(new TestUserMethod());

			context.GetCurrentUser().ShouldBe("John");
		}

		[Fact]
		public void When_querying_current_user_and_no_implementations_match()
		{
			var context = new ObjectContext(new TestTarget());

			context.GetCurrentUser().ShouldBe(string.Empty);
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

		private class TestUserProperty
		{
			public string CurrentUser { get; } = "Dave";
		}

		private class TestUserMethod
		{
			public string GetCurrentUser() => "John";
		}
	}
}
