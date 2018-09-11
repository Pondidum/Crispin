using System;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class Scratchpad
	{
		private class Toggles
		{
			public static readonly Guid Smars = Guid.NewGuid();
		}

		[Fact]
		public void When_testing_something()
		{
			var service = new ToggleService();

			var enabled = new Condition { ID = 1, ConditionType = "true" };
			var disabled = new Condition { ID = 2, ConditionType = "false" };
			var any = new Condition { ID = 0, ConditionType = "any", Children = new[] { enabled, disabled } };

			service.Populate(new[]
			{
				new Toggle { ID = Toggles.Smars, Conditions = new[] { any } }
			});

			var active = service.IsActive(Toggles.Smars, new
			{
				UserName = "testing"
			});

			active.ShouldBeTrue();
		}
	}
}
