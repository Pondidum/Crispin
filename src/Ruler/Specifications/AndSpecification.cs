namespace Ruler.Specifications
{
	public class AndSpecification<T> : AllSpecification<T>
	{
		public AndSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}
	}
}
