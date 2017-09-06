using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Events;
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
			Should.Throw<KeyNotFoundException>(
				async () => await Handler.Handle(new UpdateToggleTagsRequest(ToggleID.CreateNew()))
			);
		}

		public static IEnumerable<object[]> Tags => new[]
		{
			new[] { "existing" },
			new string [0],
			new[] { "existing", "other" },
			new[] { "what" }
		}.Select(x => new object[] { x });

		[Theory]
		[MemberData(nameof(Tags))]
		public async Task When_changing_tags_around_randomly(string[] tags)
		{
			var response = await Handler.Handle(new UpdateToggleTagsRequest(ToggleID)
			{
				Tags = tags
			});

			response.Tags.ShouldBe(tags, ignoreOrder: true);
		}
	}
}
