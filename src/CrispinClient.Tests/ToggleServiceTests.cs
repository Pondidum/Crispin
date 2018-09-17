using System;
using System.Collections.Generic;
using CrispinClient.Conditions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class ToggleServiceTests
	{
		[Fact]
		public void Scratch()
		{
			var enabled = new EnabledCondition { ID = 1 };
			var disabled = new DisabledCondition { ID = 2 };
			var any = new AnyCondition { ID = 0, Children = new Condition[] { enabled, disabled } };

			var toggle = new Toggle { ID = Guid.NewGuid(), Conditions = new[] { any } };

			var fetcher = Substitute.For<IToggleFetcher>();
			fetcher.GetAllToggles().Returns(new Dictionary<Guid, Toggle> { { toggle.ID, toggle } });
			var service = new ToggleService(fetcher);

			var active = service.IsActive(toggle.ID, new
			{
				UserName = "testing"
			});

			active.ShouldBeTrue();
		}
	}
}
