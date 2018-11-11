using System.Threading.Tasks;
using Crispin.Handlers.UpdateTags;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class AddTags : HandlerPipelineTest<AddToggleTagRequest, UpdateToggleTagsResponse>
	{
		protected override async Task<AddToggleTagRequest> When()
		{
			await CreateToggle(toggle => toggle.AddTag(Editor, "existing-tag"));

			return new AddToggleTagRequest(Editor, Locator, "new-tag");
		}

		[Fact]
		public void The_response_contains_the_toggle_id() => Response.ToggleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_contains_the_update_tags() => Response.Tags.ShouldBe(new[] { "existing-tag", "new-tag" });

		[Fact]
		public async Task The_toggle_is_saved() => (await Read(ToggleID)).Tags.ShouldBe(new[] { "existing-tag", "new-tag" });
	}
}
