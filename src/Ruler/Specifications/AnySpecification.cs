using System.Linq;

namespace Ruler.Specifications
{
	public class AnySpecification<T> : ISpecification<T>
	{
		private readonly ISpecification<T>[] _inner;

		public AnySpecification(params ISpecification<T>[] inner) => _inner = inner;

		public bool IsMatch(T input) => _inner.Any(inner => inner.IsMatch(input));
	}
}
