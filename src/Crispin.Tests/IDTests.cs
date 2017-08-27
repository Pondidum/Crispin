using System;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Crispin.Tests
{
	public abstract class IDTests<T>
	{
		protected abstract T CreateNew();
		protected abstract T Parse(Guid input);

		[Fact]
		public void When_serializing_and_deserializing()
		{
			var toggle = CreateNew();

			var json = JsonConvert.SerializeObject(toggle);
			var deserialized = JsonConvert.DeserializeObject<T>(json);

			deserialized.ShouldBe(toggle);
		}

		[Fact]
		public void When_serializing()
		{
			var guid = Guid.NewGuid();
			var toggle = Parse(guid);

			var json = JsonConvert.SerializeObject(new
			{
				Prop = toggle
			});

			json.ShouldBe($"{{\"Prop\":\"{guid}\"}}");
		}
	}
}