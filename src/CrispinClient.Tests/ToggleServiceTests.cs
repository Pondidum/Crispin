using System;
using CrispinClient.Conditions;
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

			var enabled = new EnabledCondition { ID = 1 };
			var disabled = new DisabledCondition { ID = 2 };
			var any = new AnyCondition { ID = 0, Children = new Condition[] { enabled, disabled } };

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
