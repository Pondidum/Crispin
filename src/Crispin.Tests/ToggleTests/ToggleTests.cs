using System;
using Crispin.Events;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleTests
	{
		[Fact]
		public void When_loading_from_an_event_stream()
		{
			var toggleCreated = new ToggleCreated(Guid.NewGuid(), "toggle name", "toggle desc");
			var toggle = Toggle.LoadFrom(new object[] { toggleCreated });
			
			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldBe(toggleCreated.ID),
				() => toggle.Name.ShouldBe(toggleCreated.Name),
				() => toggle.Description.ShouldBe(toggleCreated.Description)
			);
		}
	}
}
