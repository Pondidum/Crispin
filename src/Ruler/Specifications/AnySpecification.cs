using System.Collections.Generic;
using System.Linq;

namespace Ruler.Specifications
{
	public class AnySpecification<T> : ISpecification<T>
	{
		private readonly IEnumerable<ISpecification<T>> _inner;

		public AnySpecification(params ISpecification<T>[] inner) => _inner = inner;
		public AnySpecification(IEnumerable<ISpecification<T>> inner) => _inner = inner;

		public bool IsMatch(T input) => _inner.Any(inner => inner.IsMatch(input));
	}
}
