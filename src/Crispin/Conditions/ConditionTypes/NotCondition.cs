using System.Collections.Generic;
using System.Linq;

namespace Crispin.Conditions.ConditionTypes
{
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

		public void RemoveChild(ConditionID childID)
		{
			if (_child?.ID == childID)
				_child = null;
		}
	}
}
