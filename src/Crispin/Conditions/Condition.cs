using System;
using System.Collections.Generic;

namespace Crispin.Conditions
{
	public abstract class Condition
	{
		public string ConditionType => GetType().Name.Replace("Condition", "");
		public ConditionID ID { get; set; }

		public virtual IEnumerable<string> Validate() => Array.Empty<string>();
	}
}
