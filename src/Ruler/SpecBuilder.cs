using System;
using System.Collections.Generic;
using System.Linq;
using Ruler.Specifications;

namespace Ruler
{
	public class SpecBuilder<T>
	{
		private readonly Dictionary<string, Func<Func<SpecPart, ISpecification<T>>, SpecPart, ISpecification<T>>> _builders;

		public SpecBuilder()
		{
			_builders = new Dictionary<string, Func<Func<SpecPart, ISpecification<T>>, SpecPart, ISpecification<T>>>
			{
				["not"] = (build, part) => new NotSpecification<T>(build(part.Children.Single())),
				["and"] = (build, part) => new AndSpecification<T>(part.Children.Select(build)),
				["or"] = (build, part) => new OrSpecification<T>(part.Children.Select(build)),
				["all"] = (build, part) => new AllSpecification<T>(part.Children.Select(build)),
				["any"] = (build, part) => new AnySpecification<T>(part.Children.Select(build)),
				["true"] = (build, part) => new FixedSpecification<T>(true),
				["false"] = (build, part) => new FixedSpecification<T>(false)
			};
		}

		public ISpecification<T> Build(SpecPart part)
		{
			if (_builders.ContainsKey(part.Type) == false)
				throw new NotSupportedException(part.Type);

			return _builders[part.Type](Build, part);
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
