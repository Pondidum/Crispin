using System.Collections.Generic;

namespace Crispin.Rules
{
	public abstract class Condition
	{
		public int ID { get; set; }
		public abstract string Description { get; }
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
		public override string Description { get; } = "The toggle is enabled.";
	}

	public class DisabledCondition : Condition
	{
		public override string Description { get; } = "The toggle is disabled.";
	}

	public class NotCondition : Condition, ISingleChild
	{
		public override string Description { get; } = "The toggle is enabled if the child condition is not enabled.";
		public Condition Child { get; set; }
	}

	public class AnyCondition : Condition, IMultipleChildren
	{
		public override string Description { get; } = "The toggle is enabled if any child conditions are enabled.";
		public IEnumerable<Condition> Children { get; set; }
	}
	
	public class AllCondition : Condition, IMultipleChildren
	{
		public override string Description { get; } = "The toggle is enabled if all child conditions are enabled.";
		public IEnumerable<Condition> Children { get; set; }
	}

	public class InGroupCondition : Condition
	{
		public override string Description { get; } = "The toggle is enabled if the value is contained in the group.";
		public string SearchKey { get; set; }
		public string GroupName { get; set; }
	}
}
