using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Handlers.UpdateTags;
using Crispin.Infrastructure.Storage;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class UpdateToggleTagsHandlerTests : HandlerTest<UpdateToggleTagsHandler>
	{
		protected override UpdateToggleTagsHandler CreateHandler(IStorage storage)
		{
			return new UpdateToggleTagsHandler(storage);
		}

		protected override void InitialiseToggle(Toggle toggle)
		{
			toggle.AddTag("existing");
		}

		[Fact]
		public void When_adding_tags_and_the_toggle_doesnt_exist()
		{
			Should.Throw<KeyNotFoundException>(async () => await Handler.Handle(
				new AddToggleTagRequest(ToggleID.CreateNew(), "wat"))
			);
		}

		[Fact]
		public void When_removing_tags_and_the_toggle_doesnt_exist()
		{
			Should.Throw<KeyNotFoundException>(async () => await Handler.Handle(
				new RemoveToggleTagRequest(ToggleID.CreateNew(), "wat"))
			);
		}

		[Theory]
		[InlineData(TagAction.Add, "existing", "existing")]
		[InlineData(TagAction.Add, "new", "existing,new")]
		[InlineData(TagAction.Remove, "existing", "")]
		[InlineData(TagAction.Remove, "nonexisting", "existing")]
		public async Task When_changing_tags_around_randomly(TagAction action, string tagName, string expected)
		{
			var response = action == TagAction.Add
				? await Handler.Handle(new AddToggleTagRequest(ToggleID, tagName))
				: await Handler.Handle(new RemoveToggleTagRequest(ToggleID, tagName));

			var expectedTags = string.IsNullOrWhiteSpace(expected)
				? Array.Empty<string>()
				: expected.Split(",");

			response.Tags.ShouldBe(expectedTags, ignoreOrder: true);
		}

		public enum TagAction
		{
			Add,
			Remove
		}
	}
}
