using System;
using CrispinClient.Conditions;

namespace CrispinClient
{
	public class Toggle
	{
		public Guid ID { get; set; }
		public Condition[] Conditions { get; set; }

		public bool IsActive(IActiveQuery query)
		{
			var any = new AnyCondition { Children = Conditions };
			return any.IsMatch(query);
		}
	}
}
