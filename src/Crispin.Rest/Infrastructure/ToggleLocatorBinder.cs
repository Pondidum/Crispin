using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Crispin.Rest.Infrastructure
{
	public class ToggleLocatorBinder : IModelBinderProvider, IModelBinder
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType == typeof(ToggleLocator))
				return this;

			return null;
		}

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(ToggleLocator))
				return Task.CompletedTask;

			var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
			var guid = Guid.Empty;

			var locator = Guid.TryParse(value.FirstValue, out guid)
				? ToggleLocator.Create(ToggleID.Parse(guid))
				: ToggleLocator.Create(value.FirstValue);

			bindingContext.Result = ModelBindingResult.Success(locator);

			return Task.CompletedTask;
		}
	}
}
