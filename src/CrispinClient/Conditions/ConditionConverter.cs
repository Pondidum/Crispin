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
			Builder = new Dictionary<string, Func<Condition>>(StringComparer.OrdinalIgnoreCase)
			{
				{ "enabled", () => new EnabledCondition() },
				{ "on", () => new EnabledCondition() },
				{ "true", () => new EnabledCondition() },

				{ "disabled", () => new DisabledCondition() },
				{ "off", () => new DisabledCondition() },
				{ "false", () => new DisabledCondition() },

				{ "not", () => new NotCondition() },
				{ "any", () => new AnyCondition() },
				{ "all", () => new AllCondition() },

				{ "ingroup", () => new InGroupCondition() }
			};
		}

		public override bool CanWrite => false;
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();

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
