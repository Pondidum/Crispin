using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Events;
using Crispin.Handlers.UpdateTags;
using Crispin.Infrastructure.Storage;
using NSubstitute;
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
		public void When_the_toggle_doesnt_exist()
		{
			Should.Throw<KeyNotFoundException>(
				async () => await Handler.Handle(new UpdateToggleTagsRequest(ToggleID.CreateNew()))
			);
		}

		[Fact]
		public async Task When_there_are_no_tags_provided()
		{
			var response = await Handler.Handle(new UpdateToggleTagsRequest(ToggleID));

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded)
			});

			response.Tags.ShouldBe(new[] { "existing" });
		}

		[Fact]
		public async Task When_adding_a_tag()
		{
			var response = await Handler.Handle(new UpdateToggleTagsRequest(ToggleID)
			{
				Tags = new[] { "First" }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(TagAdded)
			});

			response.Tags.ShouldBe(new[] { "existing", "First" }, ignoreOrder: true);
		}

		[Fact]
		public async Task When_adding_multiple_tags()
		{
			var response = await Handler.Handle(new UpdateToggleTagsRequest(ToggleID)
			{
				Tags = new[] { "First", "Second", "Third" }
			});

			EventTypes().ShouldBe(new[]
			{
				typeof(ToggleCreated),
				typeof(TagAdded),
				typeof(TagAdded),
				typeof(TagAdded),
				typeof(TagAdded)
			});

			response.Tags.ShouldBe(new[] { "existing", "First", "Second", "Third" }, ignoreOrder: true);
		}
	}
}
