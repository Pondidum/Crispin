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

		public abstract bool IsMatch(IToggleReporter reporter, IToggleContext context);

		protected bool Report(IToggleReporter reporter, bool state)
		{
			reporter.Report(this, state);
			return state;
		}
	}

	public class EnabledCondition : Condition
	{
		public override bool IsMatch(IToggleReporter reporter, IToggleContext context)
		{
			return Report(reporter, true);
		}
	}

	public class DisabledCondition : Condition
	{
		public override bool IsMatch(IToggleReporter reporter, IToggleContext context)
		{
			return Report(reporter, false);
		}
	}

	public class NotCondition : Condition
	{
		public override bool IsMatch(IToggleReporter reporter, IToggleContext context)
		{
			return Report(reporter, Children.Single().IsMatch(reporter, context) == false);
		}
	}

	public class AnyCondition : Condition
	{
		public override bool IsMatch(IToggleReporter reporter, IToggleContext context)
		{
			return Report(reporter, Children.Any(child => child.IsMatch(reporter, context)));
		}
	}

	public class AllCondition : Condition
	{
		public override bool IsMatch(IToggleReporter reporter, IToggleContext context)
		{
			return Report(reporter, Children.All(child => child.IsMatch(reporter, context)));
		}
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }

		public override bool IsMatch(IToggleReporter reporter, IToggleContext context)
		{
			return Report(reporter, context.GroupContains(GroupName, SearchKey));
		}
	}
}
