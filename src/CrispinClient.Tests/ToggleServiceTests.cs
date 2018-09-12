using System;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class ToggleServiceTests
	{
		[Fact]
		public void Scratch()
		{
			var toggleID = Guid.NewGuid();
			var service = new ToggleService();

			var enabled = new Condition { ID = 1, ConditionType = "true" };
			var disabled = new Condition { ID = 2, ConditionType = "false" };
			var any = new Condition { ID = 0, ConditionType = "any", Children = new[] { enabled, disabled } };

			service.Populate(new[]
			{
				new Toggle { ID = toggleID, Conditions = new[] { any } }
			});

			var active = service.IsActive(toggleID, new
			{
				UserName = "testing"
			});

			active.ShouldBeTrue();
		}
	}
}
