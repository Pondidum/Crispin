namespace Ruler.Specifications
{
	public class FixedSpecification<T> : ISpecification<T>
	{
		private readonly bool _result;

		public FixedSpecification(bool result) => _result = result;

		public bool IsMatch(T input) => _result;
	}
}
