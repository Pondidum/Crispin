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

		public void Add(Condition condition, int parentConditionID)
		{
			var parent = FindCondition(parentConditionID);

			if (parent == null)
				throw new ConditionNotFoundException(parentConditionID);

			if (parent.SupportsChildren == false)
				throw new ConditionException($"{parent.GetType().Name} does not support children.");

			AddChild(parent, condition);
		}

		public void Remove(int conditionID)
		{
			if (HasCondition(conditionID) == false)
				throw new ConditionNotFoundException(conditionID);

			RemoveChild(conditionID);
		}

		public bool HasCondition(int id) => FindCondition(_conditions, id) != null;
		public Condition FindCondition(int id) => FindCondition(_conditions, id);

		private static void AddChild(Condition parent, Condition child)
		{
			if (parent is ISingleChild single)
			{
				if (single.Child != null)
					throw new ConditionException("The parent condition only supports one child, and it already has one");

				single.Child = child;
			}

			if (parent is IParentCondition multi)
				multi.AddChild(child);
		}

		private static Condition FindCondition(IEnumerable<Condition> conditions, int id)
		{
			var next = new List<Condition>();

			foreach (var condition in conditions)
			{
				if (condition.ID == id)
					return condition;

				if (condition is ISingleChild single)
					next.Add(single.Child);

				if (condition is IParentCondition multi)
					next.AddRange(multi.Children);
			}

			if (next.Any())
				return FindCondition(next, id);

			return null;
		}

		private void RemoveChild(int childId)
		{
			var removed = _conditions.RemoveAll(c => c.ID == childId);

			if (removed > 0)
				return;

			RemoveChild(_conditions, childId);
		}

		private static void RemoveChild(IEnumerable<Condition> conditions, int id)
		{
			foreach (var condition in conditions)
			{
				if (condition is ISingleChild single && single.Child.ID == id)
				{
					single.Child = null;
					return;
				}

				if (condition is IParentCondition multi)
				{
					multi.RemoveChild(id);
					RemoveChild(multi.Children, id);
				}
			}
		}
	}
}
