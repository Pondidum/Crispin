using System.Collections.Generic;
using System.Linq;

namespace Crispin.Rules
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
		IEnumerable<Condition> Children { get; set; }
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
		public IEnumerable<Condition> Children { get; set; }

		public AnyCondition()
		{
			Children = Enumerable.Empty<Condition>();
		}
	}

	public class AllCondition : Condition, IMultipleChildren
	{
		public IEnumerable<Condition> Children { get; set; }

		public AllCondition()
		{
			Children = Enumerable.Empty<Condition>();
		}
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }
	}
}
