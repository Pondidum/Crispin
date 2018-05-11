using System.Collections.Generic;
using System.Linq;

namespace Ruler.Specifications
{
	public class AllSpecification<T> : ISpecification<T>
	{
		private readonly IEnumerable<ISpecification<T>> _inner;

		public AllSpecification(params ISpecification<T>[] inner) => _inner = inner;
		public AllSpecification(IEnumerable<ISpecification<T>> inner) => _inner = inner;

		public bool IsMatch(T input) => _inner.All(inner => inner.IsMatch(input));
	}
}
