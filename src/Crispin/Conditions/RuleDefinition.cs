using System.Collections.Generic;
using System.Linq;

namespace Crispin.Conditions
{
	public abstract class Condition
	{
		public int ID { get; set; }
	}

	public interface IParentCondition
	{
		IEnumerable<Condition> Children { get; }

		bool CanAdd(Condition child);
		void AddChild(Condition child);
		void RemoveChild(int childID);
	}

	public class EnabledCondition : Condition
	{
	}

	public class DisabledCondition : Condition
	{
	}

	public class NotCondition : Condition, IParentCondition
	{
		private Condition _child;

		public IEnumerable<Condition> Children => _child == null
			? Enumerable.Empty<Condition>()
			: new[] { _child };

		public bool CanAdd(Condition child) => _child == null;

		public void AddChild(Condition child)
		{
			if (_child != null)
				throw new ConditionException("The parent condition only supports one child, and it already has one");

			_child = child;
		}

		public void RemoveChild(int childID)
		{
			if (_child?.ID == childID)
				_child = null;
		}
	}

	public class AnyCondition : Condition, IParentCondition
	{
		private readonly List<Condition> _children;

		public AnyCondition()
		{
			_children = new List<Condition>();
		}

		public IEnumerable<Condition> Children => _children;

		public bool CanAdd(Condition child) => true;
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

		public bool CanAdd(Condition child) => true;
		public void AddChild(Condition child) => _children.Add(child);
		public void RemoveChild(int childID) => _children.RemoveAll(c => c.ID == childID);
	}

	public class InGroupCondition : Condition
	{
		public string SearchKey { get; set; }
		public string GroupName { get; set; }
	}
}
