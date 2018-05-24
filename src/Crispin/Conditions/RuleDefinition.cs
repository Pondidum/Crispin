using System.Collections.Generic;

namespace Crispin.Conditions
{
	public abstract class Condition
	{
		public int ID { get; set; }

		public bool SupportsChildren => this is ISingleChild || this is IParentCondition;
	}

	public interface IParentCondition
	{
		IEnumerable<Condition> Children { get; }

		void AddChild(Condition child);
		void RemoveChild(int childID);
	}

	public interface ISingleChild
	{
		Condition Child { get; set; }
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

	public class AnyCondition : Condition, IParentCondition
	{
		private readonly List<Condition> _children;

		public AnyCondition()
		{
			_children = new List<Condition>();
		}

		public IEnumerable<Condition> Children => _children;

		public void AddChild(Condition child) => _children.Add(child);
		public void RemoveChild(int childID) => _children.RemoveAll(c => c.ID == childID);
	}

	public class AllCondition : Condition, IParentCondition
	{
		private readonly List<Condition> _children;

		public AllCondition()
		{
			_children = new List<Condition>();
		}

		public IEnumerable<Condition> Children => _children;

		public void AddChild(Condition child) => _children.Add(child);
		public void RemoveChild(int childID) => _children.RemoveAll(c => c.ID == childID);
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }
	}
}
