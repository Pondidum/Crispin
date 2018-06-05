using System.Collections.Generic;

namespace Crispin.Conditions.ConditionTypes
{
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
		public void RemoveChild(ConditionID childID) => _children.RemoveAll(c => c.ID == childID);
	}
}
