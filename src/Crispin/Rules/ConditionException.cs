using System;
using System.Runtime.Serialization;

namespace Crispin.Rules
{
	public class ConditionException : Exception
	{
		public ConditionException(string message) : base(message)
		{
		}

		protected ConditionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
