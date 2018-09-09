using System;
using System.Linq;
using Ruler;
using Ruler.Specifications;

namespace CrispinClient
{
	public class Toggle
	{
		public Guid ID { get; set; }
		public Condition[] Conditions { get; set; }

		public bool IsActive(IActiveQuery query)
		{
			var specBuilder = new SpecBuilder<IActiveQuery>();

			var specs = Conditions
				.Select(Convert)
				.Select(specBuilder.Build)
				.ToArray();

			return new AnySpecification<IActiveQuery>(specs)
				.IsMatch(query);
		}

		private SpecPart Convert(Condition condition)
		{
			return new SpecPart
			{
				Type = condition.ConditionType,
				Children = condition.Children.Select(Convert)
			};
		}
	}

	public class Condition
	{
		public int ID { get; set; }
		public string ConditionType { get; set; }
		public Condition[] Children { get; set; }
	}
}
