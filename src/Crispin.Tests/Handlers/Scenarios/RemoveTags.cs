using System.Threading.Tasks;
using Crispin.Handlers.UpdateTags;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class RemoveTags : HandlerTest<RemoveToggleTagRequest, UpdateToggleTagsResponse>
	{
		protected override async Task<RemoveToggleTagRequest> When()
		{
			await CreateToggle(toggle =>
			{
				toggle.AddTag(Editor, "tag-one");
				toggle.AddTag(Editor, "tag-two");
				toggle.AddTag(Editor, "tag-three");
			});

			return new RemoveToggleTagRequest(Editor, Locator, "tag-two");
		}

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_contains_the_update_tags() => Response.Tags.ShouldBe(new[] { "tag-one", "tag-three" });

		[Fact]
		public async Task The_toggle_is_saved() => (await Read(ToggleID)).Tags.ShouldBe(new[] { "tag-one", "tag-three" });
	}
}
