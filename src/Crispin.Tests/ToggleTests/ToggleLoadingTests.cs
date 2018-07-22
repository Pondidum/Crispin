using Crispin.Events;
using Crispin.Infrastructure;
using Shouldly;
using Xunit;

namespace Crispin.Tests.ToggleTests
{
	public class ToggleLoadingTests : ToggleTest
	{
		[Fact]
		public void When_loading_from_an_event_stream()
		{
			var toggleID = ToggleID.CreateNew();
			var toggleCreated = new ToggleCreated(
				Editor,
				toggleID,
				"toggle name",
				"toggle desc");

			Toggle = new Toggle();

			var loader = new Aggregator(Toggle.GetType());
			loader.Apply(Toggle, toggleCreated);

			Toggle.ShouldSatisfyAllConditions(
				() => Toggle.ID.ShouldBe(toggleID),
				() => Toggle.Name.ShouldBe(toggleCreated.Name),
				() => Toggle.Description.ShouldBe(toggleCreated.Description)
			);
		}
	}
}
