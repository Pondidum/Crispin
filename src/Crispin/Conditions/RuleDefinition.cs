using System.Collections.Generic;

namespace Crispin.Conditions
{
	public abstract class Condition
	{
		public int ID { get; set; }

		public bool SupportsChildren => this is ISingleChild || this is IMultipleChildren;
	}

	public interface ISingleChild
	{
		Condition Child { get; set; }
	}

	public interface IMultipleChildren
	{
		List<Condition> Children { get; set; }
	}

	public class EnabledCondition : Condition
	{
	}

	public class DisabledCondition : Condition
	{
	}

	public class NotCondition : Condition, ISingleChild
	{
		public Condition Child { get; set; }
	}

	public class AnyCondition : Condition, IMultipleChildren
	{
		public List<Condition> Children { get; set; }

		public AnyCondition()
		{
			Children = new List<Condition>();
		}
	}

	public class AllCondition : Condition, IMultipleChildren
	{
		public List<Condition> Children { get; set; }

		public AllCondition()
		{
			Children = new List<Condition>();
		}
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }
	}
}
