using System.Collections.Generic;
using System.Linq;

namespace Ruler.Specifications
{
	public class OrSpecification<T> : AnySpecification<T>
	{
		public OrSpecification(IEnumerable<ISpecification<T>> sequence)
			: base(sequence.Take(2))
		{
		}

		public OrSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}
	}
}
