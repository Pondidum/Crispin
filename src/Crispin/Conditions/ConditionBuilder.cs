using System;
using System.Collections.Generic;
using System.Linq;

namespace Crispin.Conditions
{
	public class ConditionBuilder
	{
		public IEnumerable<Condition> All => _conditions;

		private readonly List<Condition> _conditions;

		public ConditionBuilder(List<Condition> conditions = null)
		{
			_conditions = conditions ?? new List<Condition>();
		}

		public bool CanAdd(Condition child, ConditionID parentConditionID)
		{
			var condition = FindCondition(parentConditionID);

			return condition is IParentCondition parent && parent.CanAdd(child);
		}

		public void Add(Condition child, ConditionID parentConditionID = null)
		{
			if (parentConditionID == null)
			{
				_conditions.Add(child);
				return;
			}

			var condition = FindCondition(parentConditionID);

			if (condition == null)
				throw new ConditionNotFoundException(parentConditionID);

			var parent = condition as IParentCondition;

			if (parent is null)
				throw new ConditionException($"{condition.GetType().Name} does not support children.");

			parent.AddChild(child);
		}

		public void Remove(ConditionID conditionID)
		{
			if (HasCondition(conditionID) == false)
				throw new ConditionNotFoundException(conditionID);

			RemoveChild(conditionID);
		}

		public bool HasCondition(ConditionID id) => FindCondition(_conditions, id) != null;
		public Condition FindCondition(ConditionID id) => FindCondition(_conditions, id);

		private static Condition FindCondition(IEnumerable<Condition> conditions, ConditionID id)
		{
			foreach (var condition in conditions)
			{
				if (condition.ID == id)
					return condition;

				if (condition is IParentCondition multi)
				{
					var found = FindCondition(multi.Children, id);

					if (found != null)
						return found;
				}
			}

			return null;
		}

		private void RemoveChild(ConditionID childID)
		{
			var removed = _conditions.RemoveAll(c => c.ID == childID);

			if (removed > 0)
				return;

			RemoveChild(_conditions, childID);
		}

		private static void RemoveChild(IEnumerable<Condition> conditions, ConditionID id)
		{
			foreach (var parent in conditions.OfType<IParentCondition>())
			{
				parent.RemoveChild(id);
				RemoveChild(parent.Children, id);
			}
		}
	}
}
