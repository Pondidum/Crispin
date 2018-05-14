using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Crispin.Rules
{
	public class ConditionBuilder
	{
		public Condition Condition { get; private set; }

		private int _nextConditionId;

		public ConditionBuilder(int startIndex)
		{
			_nextConditionId = startIndex;
		}

		public void AddCondition(Condition condition)
		{
			if (Condition != null)
				throw new NotSupportedException("There is already a root condition");

			condition.ID = _nextConditionId;
			_nextConditionId++;

			Condition = condition;
		}

		public void RemoveCondition(int conditionID)
		{
			if (Condition?.ID == conditionID)
				Condition = null;
		}

		public void AddCondition(Condition condition, int parentCondition)
		{
			var parent = FindCondition(Condition, parentCondition);
			
			if (parent == null)
				throw new InvalidOperationException($"Could not find parent with ID '{parentCondition}'");

			if (parent is IMultipleChildren multi)
				multi.Children = (multi.Children ?? Enumerable.Empty<Condition>()).Append(condition);

			if (parent is ISingleChild single)
				single.Child = condition;
		}


		private Condition FindCondition(Condition current, int id)
		{
			if (current == null)
				return null;

			if (current.ID == id)
				return current;

			var children = new List<Condition>();

			children.Add((current as ISingleChild)?.Child);
			children.AddRange((current as IMultipleChildren)?.Children);

			foreach (var child in children)
			{
				var match = FindCondition(child, id);

				if (match != null)
					return match;
			}

			return null;
		}
	}
}
