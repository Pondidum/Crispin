using System;
using System.Runtime.Serialization;

namespace Crispin.Conditions
{
	public class ConditionNotFoundException : Exception
	{
		public ConditionNotFoundException(ConditionID conditionID)
			: base($"Unable to find a condition with the ID '{conditionID}'")
		{
		}

		protected ConditionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
