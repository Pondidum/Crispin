using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crispin.Conditions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Crispin.Rest.Infrastructure
{
	public class DomainIDBinder : IModelBinderProvider, IModelBinder
	{
		private static readonly Dictionary<Type, Func<string, object>> Builders = new Dictionary<Type, Func<string, object>>
		{
			{ typeof(GroupID), value => GroupID.Parse(value) },
			{ typeof(UserID), value => UserID.Parse(value) },
			{ typeof(ConditionID), value => ConditionID.Parse(int.Parse(value))}
		};

		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			return Builders.ContainsKey(context.Metadata.ModelType)
				? this
				: null;
		}

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (Builders.ContainsKey(bindingContext.ModelType) == false)
				return Task.CompletedTask;

			var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
			var builder = Builders[bindingContext.ModelType];

			bindingContext.Result = ModelBindingResult.Success(builder(value.FirstValue));

			return Task.CompletedTask;
		}
	}
}
