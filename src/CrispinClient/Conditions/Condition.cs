using System;
using System.Linq;
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

		public abstract bool IsMatch(IToggleContext context);
	}

	public class EnabledCondition : Condition
	{
		public override bool IsMatch(IToggleContext context) => true;
	}

	public class DisabledCondition : Condition
	{
		public override bool IsMatch(IToggleContext context) => false;
	}

	public class NotCondition : Condition
	{
		public override bool IsMatch(IToggleContext context) => Children.Single().IsMatch(context) == false;
	}

	public class AnyCondition : Condition
	{
		public override bool IsMatch(IToggleContext context) => Children.Any(child => child.IsMatch(context));
	}

	public class AllCondition : Condition
	{
		public override bool IsMatch(IToggleContext context) => Children.All(child => child.IsMatch(context));
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }

		public override bool IsMatch(IToggleContext context) => context.GroupContains(GroupName, SearchKey);
	}
}
