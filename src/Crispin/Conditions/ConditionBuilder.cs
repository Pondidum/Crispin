using System.Collections.Generic;
using System.Linq;

namespace Crispin.Conditions
{
	public class ConditionBuilder
	{
		public IEnumerable<Condition> All => _conditions;

		private readonly List<Condition> _conditions;

		public ConditionBuilder()
		{
			_conditions = new List<Condition>();
		}

		public void Add(Condition condition) => _conditions.Add(condition);

		public void Add(Condition child, int parentConditionID)
		{
			var condition = FindCondition(parentConditionID);

			if (condition == null)
				throw new ConditionNotFoundException(parentConditionID);

			var parent = condition as IParentCondition;

			if (parent is null)
				throw new ConditionException($"{condition.GetType().Name} does not support children.");

			parent.AddChild(child);
		}

		public void Remove(int conditionID)
		{
			if (HasCondition(conditionID) == false)
				throw new ConditionNotFoundException(conditionID);

			RemoveChild(conditionID);
		}

		public bool HasCondition(int id) => FindCondition(_conditions, id) != null;
		public Condition FindCondition(int id) => FindCondition(_conditions, id);

		private static Condition FindCondition(IEnumerable<Condition> conditions, int id)
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

		private void RemoveChild(int childID)
		{
			var removed = _conditions.RemoveAll(c => c.ID == childID);

			if (removed > 0)
				return;

			RemoveChild(_conditions, childID);
		}

		private static void RemoveChild(IEnumerable<Condition> conditions, int id)
		{
			foreach (var parent in conditions.OfType<IParentCondition>())
			{
				parent.RemoveChild(id);
				RemoveChild(parent.Children, id);
			}
		}
	}
}
