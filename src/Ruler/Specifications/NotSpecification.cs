namespace Ruler.Specifications
{
	public class NotSpecification<T> : ISpecification<T>
	{
		private readonly ISpecification<T> _other;

		public NotSpecification(ISpecification<T> other) => _other = other;

		public bool IsMatch(T input) => _other.IsMatch(input) == false;
	}
}
