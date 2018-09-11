using System;

namespace CrispinClient
{
	public class Condition
	{
		public int ID { get; set; }
		public string ConditionType { get; set; }
		public Condition[] Children { get; set; }

		public Condition()
		{
			Children = Array.Empty<Condition>();
		}
	}
}
