using System;
using System.Threading.Tasks;
using Crispin.Handlers.Rename;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class RenameToggle : HandlerTest<RenameToggleRequest, RenameToggleResponse>
	{
		private string _newName;

		protected override async Task<RenameToggleRequest> When()
		{
			await CreateToggle();
			_newName = Guid.NewGuid().ToString();

			return new RenameToggleRequest(Editor, Locator, _newName);
		}

		[Fact]
		public void The_response_has_the_toggle_id() => Response.ToggeleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_has_the_toggle_name() => Response.Name.ShouldBe(_newName);

		[Fact]
		public async Task The_toggle_gets_renamed() => (await Read(ToggleID)).Name.ShouldBe(_newName);
	}
}
