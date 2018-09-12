using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CrispinClient
{
	public class ConditionConverter : JsonConverter
	{
		public override bool CanWrite => false;
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

		public override bool CanConvert(Type objectType) => typeof(Condition).IsAssignableFrom(objectType);

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var currentObject = JObject.Load(reader);
			var conditionType = (string)currentObject.GetValue("ConditionType", StringComparison.OrdinalIgnoreCase);

			var condition = string.Equals(conditionType, "ingroup", StringComparison.OrdinalIgnoreCase)
				? new InGroupCondition()
				: new Condition();

			using (var childReader = currentObject.CreateReader())
				serializer.Populate(childReader, condition);

			return condition;
		}
	}
}
