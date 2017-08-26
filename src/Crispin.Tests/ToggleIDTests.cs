using System;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Crispin.Tests
{
	public class ToggleIDTests
	{
		[Fact]
		public void When_serializing_and_deserializing()
		{
			var toggle = ToggleID.CreateNew();

			var json = JsonConvert.SerializeObject(toggle);
			var deserialized = JsonConvert.DeserializeObject<ToggleID>(json);

			deserialized.ShouldBe(toggle);
		}

		[Fact]
		public void When_serializing()
		{
			var guid = Guid.NewGuid();
			var toggle = ToggleID.Parse(guid);

			var json = JsonConvert.SerializeObject(new
			{
				Prop = toggle
			});

			json.ShouldBe($"{{\"Prop\":\"{guid}\"}}");
		}
	}
}
