using System.Collections.Generic;
using System.Linq;

namespace Crispin.Rules
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
		public void Add(Condition condition, int parentConditionID) => AddChild(FindCondition(_conditions, parentConditionID), condition);
		public void Remove(int conditionID) => RemoveChild(_conditions, conditionID);

		public bool HasCondition(int id) => FindCondition(_conditions, id) != null;
		public Condition FindCondition(int id) => FindCondition(_conditions, id);

		private static void AddChild(Condition parent, Condition child)
		{
			if (parent is ISingleChild single)
				single.Child = child;

			if (parent is IMultipleChildren multi)
				multi.Children.Add(child);
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

				if (condition is IMultipleChildren multi)
					next.AddRange(multi.Children);
			}

			if (next.Any())
				return FindCondition(next, id);

			return null;
		}

		private static void RemoveChild(List<Condition> conditions, int id)
		{
			var removed = conditions.RemoveAll(c => c.ID == id);

			if (removed > 0)
				return;

			foreach (var condition in conditions)
			{
				if (condition is ISingleChild single && single.Child.ID == id)
				{
					single.Child = null;
					return;
				}

				if (condition is IMultipleChildren multi)
					RemoveChild(multi.Children, id);
			}
		}
	}
}
