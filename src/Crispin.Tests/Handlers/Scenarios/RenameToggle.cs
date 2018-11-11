using System;
using System.Threading.Tasks;
using Crispin.Handlers.Rename;
using Shouldly;
using Xunit;

namespace Crispin.Tests.Handlers.Scenarios
{
	public class RenameToggle : HandlerPipelineTest<RenameToggleRequest, RenameToggleResponse>
	{
		private string _newName;

		public override async Task InitializeAsync()
		{
			await CreateToggle();
			_newName = Guid.NewGuid().ToString();

			await Send(new RenameToggleRequest(Editor, ToggleLocator.Create(ToggleID), _newName));
		}

		[Fact]
		public void The_response_has_the_toggle_id() => Response.ToggeleID.ShouldBe(ToggleID);

		[Fact]
		public void The_response_has_the_toggle_name() => Response.Name.ShouldBe(_newName);

		[Fact]
		public async Task The_toggle_gets_renamed() => (await Read(ToggleID)).Name.ShouldBe(_newName);
	}
}
