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
	public class DomainIDBinderTests
	{
		private readonly DomainIDBinder _binder;

		public DomainIDBinderTests()
		{
			_binder = new DomainIDBinder();
		}

		[Fact]
		public async Task When_parsing_a_type_not_supported()
		{
			var context = BuildContext<int>("wefewfwf");

			await _binder.BindModelAsync(context);

			context.Result.ShouldBe(ModelBindingResult.Failed());
		}

		[Fact]
		public async Task When_parsing_a_valid_user_id()
		{
			var id = Guid.NewGuid().ToString();
			var context = BuildContext<UserID>(id);

			await _binder.BindModelAsync(context);

			context.Result.IsModelSet.ShouldBeTrue();
			context.Result.Model.ShouldBe(UserID.Parse(id));
		}

		[Fact]
		public async Task When_parsing_a_valid_group_id()
		{
			var id = Guid.NewGuid().ToString();
			var context = BuildContext<GroupID>(id);

			await _binder.BindModelAsync(context);

			context.Result.IsModelSet.ShouldBeTrue();
			context.Result.Model.ShouldBe(GroupID.Parse(id));
		}

		private static DefaultModelBindingContext BuildContext<T>(string value)
		{
			var metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(T)));
			var values = Substitute.For<IValueProvider>();
			values.GetValue(Arg.Any<string>()).Returns(new ValueProviderResult(value));

			return new DefaultModelBindingContext
			{
				ModelMetadata = metadata,
				ValueProvider = values
			};
		}
	}
}
