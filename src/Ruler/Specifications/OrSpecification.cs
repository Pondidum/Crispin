namespace Ruler.Specifications
{
	public class OrSpecification<T> : AnySpecification<T>
	{
		public OrSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}
	}
}
