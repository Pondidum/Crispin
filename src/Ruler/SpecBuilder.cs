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
			_builders = new Dictionary<string, Func<Func<SpecPart, ISpecification<T>>, SpecPart, ISpecification<T>>>();

			Add("not", (build, part) => new NotSpecification<T>(build(part.Children.Single())));
			Add("and", (build, part) => new AndSpecification<T>(part.Children.Select(build)));
			Add("or", (build, part) => new OrSpecification<T>(part.Children.Select(build)));
			Add("all", (build, part) => new AllSpecification<T>(part.Children.Select(build)));
			Add("any", (build, part) => new AnySpecification<T>(part.Children.Select(build)));
			Add("true", (build, part) => new FixedSpecification<T>(true));
			Add("false", (build, part) => new FixedSpecification<T>(false));
		}

		public void Add(string name, Func<Func<SpecPart, ISpecification<T>>, SpecPart, ISpecification<T>> builder)
		{
			_builders[name] = builder;
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
