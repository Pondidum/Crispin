using System;
using System.Collections.Generic;
using CrispinClient.Conditions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CrispinClient
{
	public class ConditionConverter : JsonConverter
	{
		private static readonly Dictionary<string, Func<Condition>> Builder;

		static ConditionConverter()
		{
			Builder = new Dictionary<string, Func<Condition>>(StringComparer.OrdinalIgnoreCase);

			Builder.Add("enabled", () => new EnabledCondition());
			Builder.Add("on", () => new EnabledCondition());
			Builder.Add("true", () => new EnabledCondition());

			Builder.Add("disabled", () => new DisabledCondition());
			Builder.Add("off", () => new DisabledCondition());
			Builder.Add("false", () => new DisabledCondition());

			Builder.Add("not", () => new NotCondition());
			Builder.Add("any", () => new AnyCondition());
			Builder.Add("all", () => new AllCondition());
			
			Builder.Add("ingroup", () => new InGroupCondition());
		}

		public override bool CanWrite => false;
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

		public override bool CanConvert(Type objectType) => typeof(Condition).IsAssignableFrom(objectType);

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var currentObject = JObject.Load(reader);
			var conditionType = (string)currentObject.GetValue("ConditionType", StringComparison.OrdinalIgnoreCase);

			var condition = Builder[conditionType]();

			using (var childReader = currentObject.CreateReader())
				serializer.Populate(childReader, condition);

			return condition;
		}
	}
}
