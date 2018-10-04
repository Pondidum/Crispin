using System;
using System.Linq;
using CrispinClient.Statistics;
using Newtonsoft.Json;

namespace CrispinClient.Conditions
{
	[JsonConverter(typeof(ConditionConverter))]
	public abstract class Condition
	{
		public int ID { get; set; }
		public Condition[] Children { get; set; }

		protected Condition()
		{
			Children = Array.Empty<Condition>();
		}

		public abstract bool IsMatch(Statistic stats, IToggleContext context);

		protected bool Report(Statistic stats, bool state)
		{
			stats.Add(this, state);
			return state;
		}
	}

	public class EnabledCondition : Condition
	{
		public override bool IsMatch(Statistic stats, IToggleContext context)
		{
			return Report(stats, true);
		}
	}

	public class DisabledCondition : Condition
	{
		public override bool IsMatch(Statistic stats, IToggleContext context)
		{
			return Report(stats, false);
		}
	}

	public class NotCondition : Condition
	{
		public override bool IsMatch(Statistic stats, IToggleContext context)
		{
			return Report(stats, Children.Single().IsMatch(stats, context) == false);
		}
	}

	public class AnyCondition : Condition
	{
		public override bool IsMatch(Statistic stats, IToggleContext context)
		{
			return Report(stats, Children.Any(child => child.IsMatch(stats, context)));
		}
	}

	public class AllCondition : Condition
	{
		public override bool IsMatch(Statistic stats, IToggleContext context)
		{
			return Report(stats, Children.All(child => child.IsMatch(stats, context)));
		}
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }

		public override bool IsMatch(Statistic stats, IToggleContext context)
		{
			return Report(stats, context.GroupContains(GroupName, SearchKey));
		}
	}
}
