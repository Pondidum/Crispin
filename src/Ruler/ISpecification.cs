namespace Ruler
{
	public interface ISpecification<in T>
	{
		bool IsMatch(T input);
	}
}
