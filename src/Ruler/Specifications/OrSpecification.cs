namespace Ruler.Specifications
{
	public class OrSpecification<T> : ISpecification<T>
	{
		private readonly ISpecification<T> _left;
		private readonly ISpecification<T> _right;

		public OrSpecification(ISpecification<T> left, ISpecification<T> right)
		{
			_left = left;
			_right = right;
		}

		public bool IsMatch(T input) => _left.IsMatch(input) || _right.IsMatch(input);
	}
}
