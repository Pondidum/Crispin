using System;
using System.Threading.Tasks;
using Crispin.Handlers.Rename;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers
{
	public class RenameToggleHandlerTests : HandlerPipelineTest<RenameToggleRequest, RenameToggleResponse>
	{
		private ToggleID _toggleID;
		private string _newName;

		public override async Task InitializeAsync()
		{
			_newName = Guid.NewGuid().ToString();
			_toggleID = await CreateToggle();

			await Send(new RenameToggleRequest(Editor, ToggleLocator.Create(_toggleID), _newName));
		}

		[Fact]
		public void The_response_has_the_toggle_id() => Response.ToggeleID.ShouldBe(_toggleID);

		[Fact]
		public void The_response_has_the_toggle_name() => Response.Name.ShouldBe(_newName);

		[Fact]
		public async Task The_toggle_gets_renamed() => (await Read(_toggleID)).Name.ShouldBe(_newName);
	}
}
