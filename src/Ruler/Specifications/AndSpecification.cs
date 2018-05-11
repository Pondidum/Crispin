using System.Collections.Generic;
using System.Linq;

namespace Ruler.Specifications
{
	public class AndSpecification<T> : AllSpecification<T>
	{
		public AndSpecification(IEnumerable<ISpecification<T>> sequence)
			: base(sequence.Take(2))
		{
		}

		public AndSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}
	}
}
