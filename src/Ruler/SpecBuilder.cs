using System;
using System.Collections.Generic;
using System.Linq;
using Ruler.Specifications;

namespace Ruler
{
	public class SpecBuilder<T>
	{
		public SpecBuilder()
		{
			//_builders = new Dictionary<string, Func<SpecPart>
		}

		public ISpecification<T> Build(SpecPart part)
		{
			switch (part.Type)
			{
				case "not": return new NotSpecification<T>(part.Children.Select(Build).Single());
				case "and": return new AndSpecification<T>(part.Children.Select(Build));
				case "or": return new OrSpecification<T>(part.Children.Select(Build));
				case "all": return new AllSpecification<T>(part.Children.Select(Build));
				case "any": return new AnySpecification<T>(part.Children.Select(Build));
				case "true": return new FixedSpecification<T>(true);
				case "false": return new FixedSpecification<T>(false);
			}

			throw new NotSupportedException(part.Type);
		}
	}

	public class SpecPart
	{
		public string Type { get; set; }
		public IEnumerable<SpecPart> Children { get; set; }

		public SpecPart()
		{
			Children = Enumerable.Empty<SpecPart>();
		}
	}
}
