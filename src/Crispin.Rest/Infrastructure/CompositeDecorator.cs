using System.Linq;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;

namespace Crispin.Rest.Infrastructure
{
	public class CompositeDecorator<TComposite, TDependents> : IRegistrationConvention
		where TComposite : TDependents
	{
		public void ScanTypes(TypeSet types, Registry registry)
		{
			var dependents = types
				.FindTypes(TypeClassification.Concretes)
				.Where(t => t.CanBeCastTo<TDependents>() && t.HasConstructors())
				.Where(t => t != typeof(TComposite))
				.ToList();

			registry
				.For<TDependents>()
				.Use<TComposite>()
				.EnumerableOf<TDependents>()
				.Contains(x => dependents.ForEach(t => x.Type(t)));
		}
	}
}