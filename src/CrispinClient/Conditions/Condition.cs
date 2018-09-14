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

		public abstract bool IsMatch(IToggleContext query);
	}

	public class EnabledCondition : Condition
	{
		public override bool IsMatch(IToggleContext query) => true;
	}

	public class DisabledCondition : Condition
	{
		public override bool IsMatch(IToggleContext query) => false;
	}

	public class NotCondition : Condition
	{
		public override bool IsMatch(IToggleContext query) => Children.Single().IsMatch(query) == false;
	}

	public class AnyCondition : Condition
	{
		public override bool IsMatch(IToggleContext query) => Children.Any(child => child.IsMatch(query));
	}

	public class AllCondition : Condition
	{
		public override bool IsMatch(IToggleContext query) => Children.All(child => child.IsMatch(query));
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }

		public override bool IsMatch(IToggleContext query) => query.GroupContains(GroupName, SearchKey);
	}
}
