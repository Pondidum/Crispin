using System.Collections.Generic;

namespace Crispin.Conditions
{
	public interface IParentCondition
	{
		IEnumerable<Condition> Children { get; }

		bool CanAdd(Condition child);
		void AddChild(Condition child);
		void RemoveChild(ConditionID childID);
	}
}
