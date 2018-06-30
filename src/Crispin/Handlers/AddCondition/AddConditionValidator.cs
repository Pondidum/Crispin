using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crispin.Conditions;
using Crispin.Infrastructure.Validation;

namespace Crispin.Handlers.AddCondition
{
	public class AddConditionValidator : IRequestValidator<AddToggleConditionRequest>
	{
		private readonly IConditionBuilder _builder;

		public AddConditionValidator(IConditionBuilder builder)
		{
			_builder = builder;
		}

		public async Task<ICollection<string>> Validate(AddToggleConditionRequest request)
		{
			var builderMessages = _builder.CanCreateFrom(request.Properties).ToArray();

			if (builderMessages.Any())
				return builderMessages;

			var condition = _builder.CreateCondition(request.Properties);

			var conditionMessages = condition.Validate().ToArray();

			if (conditionMessages.Any())
				return conditionMessages;

			request.Condition = condition;
			return Array.Empty<string>();
		}
	}
}
