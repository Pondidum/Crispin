using System;
using System.Threading.Tasks;
using Crispin.Rest.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Crispin.Rest.Tests.Infrastructure
{
	public class ToggleLocatorBinderTests
	{
		private readonly ToggleLocatorBinder _binder;

		public ToggleLocatorBinderTests()
		{
			_binder = new ToggleLocatorBinder();
		}

		[Fact]
		public async Task When_parameter_is_no_a_toggle_locator()
		{
			var metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(int)));
			var context = new DefaultModelBindingContext
			{
				ModelMetadata = metadata
			};

			await _binder.BindModelAsync(context);

			context.Result.ShouldBe(ModelBindingResult.Failed());
		}

		[Fact]
		public async Task When_parameter_value_is_valid_guid()
		{
			var metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(ToggleLocator)));
			var values = Substitute.For<IValueProvider>();
			values.GetValue(Arg.Any<string>()).Returns(new ValueProviderResult(Guid.NewGuid().ToString()));

			var context = new DefaultModelBindingContext
			{
				ModelMetadata = metadata,
				ValueProvider = values
			};

			await _binder.BindModelAsync(context);

			context.Result.IsModelSet.ShouldBeTrue();
			context.Result.Model.ShouldBeOfType<ToggleLocatorByID>();
		}

		[Fact]
		public async Task When_parameter_value_is_a_string()
		{
			var metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(ToggleLocator)));
			var values = Substitute.For<IValueProvider>();
			values.GetValue(Arg.Any<string>()).Returns(new ValueProviderResult("wat"));

			var context = new DefaultModelBindingContext
			{
				ModelMetadata = metadata,
				ValueProvider = values
			};

			await _binder.BindModelAsync(context);

			context.Result.IsModelSet.ShouldBeTrue();
			context.Result.Model.ShouldBeOfType<ToggleLocatorByName>();
		}
	}
}
