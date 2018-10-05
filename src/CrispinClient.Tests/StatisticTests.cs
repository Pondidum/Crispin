using System.Collections.Generic;
using CrispinClient.Conditions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace CrispinClient.Tests
{
	public class StatisticTests
	{
		[Fact]
		public void Condition_states_serialize_correctly()
		{
			var stats = new Statistic();
			stats.Add(new EnabledCondition { ID = 17 }, true);
			stats.Add(new DisabledCondition { ID = 13 }, false);

			var token = JObject.Parse(JsonConvert.SerializeObject(stats));
			var conditions = token[nameof(stats.ConditionStates)].ToObject<Dictionary<int, bool>>();

			conditions.ShouldBe(new Dictionary<int, bool>
			{
				{ 17, true },
				{ 13, false }
			});
		}
	}
}
