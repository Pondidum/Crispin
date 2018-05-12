using System;
using NSubstitute;
using Ruler.Specifications;
using Shouldly;
using Xunit;

namespace Ruler.Tests
{
	public class SpecBuilderTests
	{
		private readonly SpecBuilder<string> _builder;

		public SpecBuilderTests()
		{
			_builder = new SpecBuilder<string>();
		}

		[Fact]
		public void When_a_spec_type_is_unknown()
		{
			Should.Throw<NotSupportedException>(() => _builder.Build(new SpecPart { Type = "wat" }));
		}

		[Fact]
		public void When_building_an_any_spec()
		{
			var spec = _builder.Build(new SpecPart { Type = "any" });

			spec.ShouldBeOfType<AnySpecification<string>>();
		}

		[Fact]
		public void When_building_an_any_spec_with_two_children()
		{
			var dto = new SpecPart
			{
				Type = "and",
				Children = new[]
				{
					new SpecPart { Type = "true" },
					new SpecPart { Type = "false" }
				}
			};

			var spec = _builder.Build(dto);
			spec.ShouldBeOfType<AndSpecification<string>>();
			spec.IsMatch("wat").ShouldBeFalse();
		}

		[Fact]
		public void Additional_specificatons_can_be_added()
		{
			var custom = Substitute.For<ISpecification<string>>();
			_builder.Add("custom", (build, part) => custom);

			var spec = _builder.Build(new SpecPart { Type = "custom" });

			spec.ShouldBe(custom);
		}
	}
}
